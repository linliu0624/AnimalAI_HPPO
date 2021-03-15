#------------#
### 上位層 ###
#------------#
import tensorflow as tf
from tensorflow.contrib.distributions import Normal
import numpy as np
import matplotlib.pyplot as plt

# criticのdiscount値
GAMMA = 0.9
# actor部の学習率
A_LR = 1e-6
# critic部の学習率
C_LR = 2e-6
# 同じデータでネットワークを更新される回数
A_UPDATE_STEPS = 50
C_UPDATE_STEPS = 50
CLIP_EPSILON = 0.2


class MetaController(object):
    def __init__(self, input_image_channel, input_pos_channel, input_dir_channel,  input_ray_channel, output_channel=28, gamma=GAMMA, image_size=90):
        self.sess = tf.Session()

        self.input_image_channel = input_image_channel
        self.input_pos_channel = input_pos_channel
        self.input_dir_channel = input_dir_channel
        self.input_ray_channel = input_ray_channel
        self.output_channel = output_channel
        self.gamma = gamma

        self.input_image_state = tf.placeholder(
            tf.float32, [None, image_size, image_size, self.input_image_channel], 'mc_input_image_state')
        self.input_pos_state = tf.placeholder(
            tf.float32, [None, self.input_pos_channel], 'mc_input_pos_state')
        self.input_dir_state = tf.placeholder(
            tf.float32, [None, self.input_dir_channel], 'mc_input_dir_state')
        self.input_ray_state = tf.placeholder(
            tf.float32, [None, self.input_ray_channel], 'mc_input_ray_state')

        self.target = tf.placeholder(
            tf.float32, [None, self.output_channel**2], 'mc_target')

        self.tfadv = tf.placeholder(tf.float32, [None, 1], 'mc_advantage')

        ### critic ###
        self.discounted_reward = tf.placeholder(
            tf.float32, [None, 1], 'mc_discounted_r')
        # critic分のニューラルネットワーク
        with tf.variable_scope('mc_critic'):
            ######  neural network ######
            x = tf.keras.layers.Conv2D(filters=32,
                                       kernel_size=[5, 5], strides=3, padding='SAME', activation=None, kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_image_state)
            x = tf.keras.layers.BatchNormalization()(x)
            x = tf.keras.layers.LeakyReLU()(x)
            x = tf.keras.layers.MaxPool2D(
                pool_size=(2, 2), strides=2, padding='same')(x)
            x = tf.keras.layers.Conv2D(filters=64,
                                       kernel_size=[3, 3], strides=1, padding='SAME', activation=None, kernel_initializer=tf.keras.initializers.VarianceScaling)(x)
            x = tf.keras.layers.BatchNormalization()(x)
            x = tf.keras.layers.LeakyReLU()(x)
            x = tf.keras.layers.Conv2D(filters=64,
                                       kernel_size=[3, 3], padding='SAME', activation=None, kernel_initializer=tf.keras.initializers.VarianceScaling)(x)
            x = tf.keras.layers.BatchNormalization()(x)
            x = tf.keras.layers.LeakyReLU()(x)
            x = tf.keras.layers.Reshape(target_shape=(14400,))(x)
            x = tf.keras.layers.Dense(
                1000, activation=tf.nn.leaky_relu, kernel_initializer=tf.keras.initializers.VarianceScaling)(x)

            x = tf.keras.layers.Dropout(0.3)(x)
            # dir
            y = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_dir_state)
            # pos
            z = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_pos_state)
            # ray
            r = tf.keras.layers.Dense(1024, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_ray_state)
            r = tf.keras.layers.Dense(512, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(r)

            xyzr = tf.keras.layers.concatenate([x, y, z, r])
            xyzr = tf.keras.layers.Dense(500, activation=tf.nn.leaky_relu,
                                         kernel_initializer=tf.keras.initializers.VarianceScaling)(xyzr)
            self.critic_output_value = tf.keras.layers.Dense(1)(xyzr)
            # TD errorの計算, この値でactorのpolicyを評価する
            self.advantage = self.discounted_reward - self.critic_output_value
            self.critic_loss = tf.reduce_mean(tf.square(self.advantage))
            self.critic_train_op = tf.train.GradientDescentOptimizer(
                C_LR).minimize(self.critic_loss)

        ### actor ###
        # 下の部分はppoアルゴリズムの核心なので，ppoを理解する必要がある
        # 新policyとネットワーク
        self.pi, pi_params = self._build_anet('mc_pi', trainable=True)
        # 旧policyとネットワーク
        self.old_pi, oldpi_params = self._build_anet(
            'mc_oldpi', trainable=False)

        with tf.variable_scope('mc_update_oldpi'):
            # 旧ネットワークを更新する(新ネットワークで上書きする)
            self.update_oldpi_op = [oldp.assign(
                p) for p, oldp in zip(pi_params, oldpi_params)]

        with tf.variable_scope('mc_loss'):
            with tf.variable_scope('mc_surrogate'):
                # 新policyの確率(部分目標の確率)
                tmp_target_prob = tf.multiply(self.pi, self.target)
                # 旧policyの確率(部分目標の確率)
                tmp_old_target_prob = tf.multiply(self.old_pi, self.target)

                # probを[None, 1]の形式にする
                self.p_target_prob = tf.reduce_sum(
                    tmp_target_prob, axis=1, keepdims=True)
                self.oldp_target_prob = tf.reduce_sum(
                    tmp_old_target_prob, axis=1, keepdims=True)

                # PPOアルゴリズムの核心
                self.ratio = tf.exp(
                    tf.log(self.p_target_prob) - tf.log(self.oldp_target_prob))
                self.surr = self.ratio * self.tfadv
                self.actor_loss = -tf.reduce_mean(tf.minimum(
                    self.surr,
                    tf.clip_by_value(self.ratio, 1.0-CLIP_EPSILON, 1.0+CLIP_EPSILON)*self.tfadv))

        with tf.variable_scope('mc_atrain'):
            # optimaier
            # self.atrain_op = tf.train.AdamOptimizer(
            #     A_LR).minimize(self.actor_loss)
            self.atrain_op = tf.train.GradientDescentOptimizer(
                A_LR).minimize(self.actor_loss)

        # tf.summary.FileWriter("log/", self.sess.graph)

        self.sess.run(tf.global_variables_initializer())
        self.saver = tf.compat.v1.train.Saver(max_to_keep=5)

    # ニューラルネットワークの更新
    def update(self, state_image, state_pos, state_dir, state_ray, target, r):
        # print("update_oldpi_op")
        self.sess.run(self.update_oldpi_op)
        # print("adv")
        # cirictにあるadvを算出する
        adv = self.sess.run(self.advantage, feed_dict={
                            self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_ray_state: state_ray,
                            self.discounted_reward: r})
        # adv = (adv - adv.mean())/(adv.std()+1e-6)     # sometimes helpful
        ### update actor ###
        [self.sess.run(self.atrain_op, {
            self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_ray_state: state_ray,
            self.target: target, self.tfadv: adv}) for _ in range(A_UPDATE_STEPS)]

        # Get actor update info and show them
        loss, ratio, surr, x1 = self.sess.run([self.actor_loss, self.ratio, self.surr, self.getinfo], {
            self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_ray_state: state_ray,
            self.target: target, self.tfadv: adv})
        # print('------ update meta controller info ------')
        # print('x1:', x1)
        # print('x1:', np.mean(x1))
        # print('mc_actor loss:', loss)
        # print('mc_ratio[0]', ratio[0])
        # print('mc_adv:', np.mean(adv))
        # print('mc_surr:', np.mean(surr))

        ### update critic ###
        # print("critic_train_op")
        [self.sess.run(self.critic_train_op, {self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_ray_state: state_ray,
                                              self.discounted_reward: r})for _ in range(C_UPDATE_STEPS)]

    # actor部のニューラルネットワーク
    def _build_anet(self, name, trainable):
        with tf.variable_scope(name):
            ######  neural network ######
            x = tf.keras.layers.Conv2D(filters=32,
                                       kernel_size=[5, 5], strides=3, padding='SAME', activation=None, trainable=trainable, kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_image_state)
            self.getinfo = x
            x = tf.keras.layers.BatchNormalization()(x)
            x = tf.keras.layers.LeakyReLU()(x)
            x = tf.keras.layers.MaxPool2D(
                pool_size=(2, 2), strides=2, padding='same')(x)
            x = tf.keras.layers.Conv2D(filters=64,
                                       kernel_size=[3, 3], strides=1, padding='SAME', activation=None, trainable=trainable, kernel_initializer=tf.keras.initializers.VarianceScaling)(x)
            x = tf.keras.layers.BatchNormalization()(x)
            x = tf.keras.layers.LeakyReLU()(x)
            x = tf.keras.layers.Conv2D(filters=64,
                                       kernel_size=[3, 3], padding='SAME', activation=None, trainable=trainable, kernel_initializer=tf.keras.initializers.VarianceScaling)(x)
            x = tf.keras.layers.BatchNormalization()(x)
            x = tf.keras.layers.LeakyReLU()(x)
            x = tf.keras.layers.Reshape(target_shape=(14400,))(x)
            x = tf.keras.layers.Dense(
                4096, activation=tf.nn.leaky_relu, trainable=trainable, kernel_initializer=tf.keras.initializers.VarianceScaling)(x)

            x = tf.keras.layers.Dropout(0.3)(x)
            # dir
            y = tf.keras.layers.Dense(1024, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_dir_state)

            # pos
            z = tf.keras.layers.Dense(1024, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_pos_state)

            # ray
            r = tf.keras.layers.Dense(2048, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_ray_state)
            r = tf.keras.layers.Dense(1024, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(r)

            xyzr = tf.keras.layers.concatenate([x, y, z, r])
            xyzr = tf.keras.layers.Dense(28*28*2, activation=tf.nn.leaky_relu,
                                         kernel_initializer=tf.keras.initializers.VarianceScaling)(xyzr)
            xyzr = tf.keras.layers.Dense(28*28, activation=tf.nn.leaky_relu,
                                         kernel_initializer=tf.keras.initializers.VarianceScaling)(xyzr)

            self.target_prob = tf.keras.layers.Softmax()(xyzr)

        params = tf.get_collection(tf.GraphKeys.GLOBAL_VARIABLES, scope=name)
        return self.target_prob, params

    # 部分目標を算出する
    def GetTargetLabel(self, state_image, state_pos, state_dir, state_ray):
        target = self.sess.run([self.target_prob], feed_dict={
            self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_ray_state: state_ray})[0][0]
        print('------ get target label info ------')
        # action_prob = self.sess.run(self.pi, feed_dict={
        #     self.input_image_state: state_image})[0]
        # print(f'target:{target}')
        sampled_target = np.random.choice(range(28*28), 1, p=target)
        return sampled_target, target

    # 部分目標を座標に変換する
    def Target2Coordinate(self, target):
        target += 1

        coord = np.zeros(3)
        coord[0] = (int(target/28))-14  # x
        coord[1] = 0.1  # y
        coord[2] = (target % 28)-14  # z
        return coord
　　
　　# 部分目標をonehotにする
    def TargetOneHot(self, target):
        target_onthot = np.zeros(28*28)
        target_onthot[target] = 1
        return target_onthot

    # critic部の計算
    def get_v(self, state_image, state_pos, state_dir, state_ray):
        value = self.sess.run(self.critic_output_value,  feed_dict={
                              self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_ray_state: state_ray})[0, 0]
        print(f'mc value:{value}')
        return value

    def SaveModel(self, learn_step_counter, dir, name="test"):
        print("-----save meta controller model-----")
        self.saver.save(self.sess, dir + name +
                        '-mc', global_step=learn_step_counter)

    def LoadModel(self, dir):
        print("-----load meta controller model-----")
        checkpoint = tf.train.get_checkpoint_state(dir)
        if checkpoint and checkpoint.model_checkpoint_path:
            self.saver.restore(self.sess, checkpoint.model_checkpoint_path)
            print("Successfully loaded:", checkpoint.model_checkpoint_path)
        else:
            print("Could not find old network weights")
