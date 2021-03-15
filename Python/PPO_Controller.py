#-----------#
### 下位層 ###
# #-----------#
# 基本的には上位層と同じなので(入出力項目だけが変わった)
# PPO_MetaController.pyを参考しよう
import tensorflow as tf
import numpy as np
import matplotlib.pyplot as plt
import gym

GAMMA = 0.9
# A_LR = 0.0001
# C_LR = 0.0002
A_LR = 1e-7
C_LR = 2e-7
A_UPDATE_STEPS = 10
C_UPDATE_STEPS = 10
CLIP_EPSILON = 0.2


class Controller(object):
    def __init__(self, input_image_channel, input_target_channel, input_pos_channel, input_dir_channel, input_dot_channel, output_channel, gamma=GAMMA):
        self.sess = tf.Session()

        self.input_image_channel = input_image_channel
        self.input_target_channel = input_target_channel
        self.input_pos_channel = input_pos_channel
        self.input_dir_channel = input_dir_channel
        self.input_dot_channel = input_dot_channel

        self.output_channel = output_channel
        self.gamma = gamma

        self.input_image_state = tf.placeholder(
            tf.float32, [None, 90, 90, self.input_image_channel], 'input_image_state')
        self.input_target_state = tf.placeholder(
            tf.float32, [None, self.input_target_channel], 'c_input_target_state')
        self.input_pos_state = tf.placeholder(
            tf.float32, [None, self.input_pos_channel], 'c_input_pos_state')
        self.input_dir_state = tf.placeholder(
            tf.float32, [None, self.input_dir_channel], 'c_input_dir_state')
        self.input_dot_state = tf.placeholder(
            tf.float32, [None, self.input_dot_channel], 'c_input_dot_state')

        self.action = tf.placeholder(
            tf.float32, [None, self.output_channel], 'c_action')
        self.tfadv = tf.placeholder(tf.float32, [None, 1], 'c_advantage')

        # critic
        self.discounted_reward = tf.placeholder(
            tf.float32, [None, 1], 'c_discounted_r')
        with tf.variable_scope('c_critic'):
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
            # target
            t = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_target_state)
            # dir
            y = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_dir_state)
            # pos
            z = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_pos_state)
            # dot
            d = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_dot_state)
            xyztd = tf.keras.layers.concatenate([x, y, z, t, d])
            xyztd = tf.keras.layers.Dense(
                300, kernel_initializer=tf.keras.initializers.VarianceScaling)(xyztd)

            self.critic_output_value = tf.keras.layers.Dense(1)(xyztd)

            self.advantage = self.discounted_reward - self.critic_output_value
            # self.advantage = tf.subtract(self.discounted_reward, self.critic_output_value)
            self.critic_loss = tf.reduce_mean(tf.square(self.advantage))
            # self.critic_train_op = tf.train.AdamOptimizer(
            #     C_LR).minimize(self.critic_loss)
            self.critic_train_op = tf.train.GradientDescentOptimizer(C_LR).minimize(self.critic_loss)

        # actor
        self.pi, pi_params = self._build_anet('c_pi', trainable=True)
        self.old_pi, oldpi_params = self._build_anet(
            'c_oldpi', trainable=False)

        with tf.variable_scope('c_update_oldpi'):
            self.update_oldpi_op = [oldp.assign(
                p) for p, oldp in zip(pi_params, oldpi_params)]

        with tf.variable_scope('c_loss'):
            with tf.variable_scope('c_surrogate'):
                # 取的pi和old_pi輸出的機率表中, 實際採取的action的對應機率
                self.tmp_p_action_prob = tf.multiply(
                    self.pi, self.action)
                self.tmp_oldp_action_prob = tf.multiply(
                    self.old_pi, self.action)
                # 把prob改成[None,1], 原本是[None,5]
                self.p_action_prob = tf.reduce_sum(
                    self.tmp_p_action_prob, axis=1, keepdims=True)
                self.oldp_action_prob = tf.reduce_sum(
                    self.tmp_oldp_action_prob, axis=1, keepdims=True)

                self.ratio = tf.exp(
                    tf.log(self.p_action_prob) - tf.log(self.oldp_action_prob))
                ### 上下幾乎一樣意思 ###
                # self.ratio = self.p_action_prob / \
                #     (self.oldp_action_prob + 1e-5)

                self.surr = self.ratio * self.tfadv
                self.actor_loss = -tf.reduce_mean(tf.minimum(
                    self.surr,
                    tf.clip_by_value(self.ratio, 1.0-CLIP_EPSILON, 1.0+CLIP_EPSILON)*self.tfadv))

        with tf.variable_scope('c_atrain'):
            # self.atrain_op = tf.train.AdamOptimizer(
            #     A_LR).minimize(self.actor_loss)
            self.atrain_op = tf.train.GradientDescentOptimizer(
                A_LR).minimize(self.actor_loss)

        # tf.summary.FileWriter("log/", self.sess.graph)

        self.sess.run(tf.compat.v1.global_variables_initializer())
        self.saver = tf.compat.v1.train.Saver(max_to_keep=5)

    def update(self, state_image, target, state_pos, state_dir, state_dot, action, reward):
        # print("update_oldpi_op")
        self.sess.run(self.update_oldpi_op)
        # print("adv")
        adv, q1, q = self.sess.run([self.advantage, self.discounted_reward, self.critic_output_value], feed_dict={
            self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_dot_state: state_dot,
            self.input_target_state: target, self.discounted_reward: reward})
        # adv = (adv - adv.mean())/(adv.std()+1e-6)     # sometimes helpful

        # update actor
        [self.sess.run(self.atrain_op, {
            self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_dot_state: state_dot,
            self.input_target_state: target, self.action: action, self.tfadv: adv}) for _ in range(A_UPDATE_STEPS)]

        # show actor info
        loss, ratio, surr = self.sess.run([self.actor_loss, self.ratio, self.surr],
                                          feed_dict={self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_dot_state: state_dot, self.input_target_state: target, self.action: action, self.tfadv: adv})
        print('------ update controller info ------')
        print('actor loss:', loss)
        print('ratio:', np.mean(ratio))
        print('adv:', np.mean(adv))
        print('surr:', np.mean(surr))
        print(f'q1:{q1[0]}, q:{q[0]}')
        # print(f'q:\n{q}')
        # print(f'q1:\n{q1}')

        # update critic
        [self.sess.run(self.critic_train_op, feed_dict={
                       self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_dot_state: state_dot,
                       self.input_target_state: target, self.discounted_reward: reward})for _ in range(C_UPDATE_STEPS)]

    def _build_anet(self, name, trainable):
        with tf.variable_scope(name):
            ######  neural network ######
            x = tf.keras.layers.Conv2D(filters=32,
                                       kernel_size=[5, 5], strides=3, padding='SAME', activation=None, trainable=trainable, kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_image_state)
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
                1000, activation=tf.nn.leaky_relu, trainable=trainable, kernel_initializer=tf.keras.initializers.VarianceScaling)(x)

            x = tf.keras.layers.Dropout(0.3)(x)
            # target
            t = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_target_state)
            # dir
            y = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_dir_state)
            # pos
            z = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_pos_state)
            # dot
            d = tf.keras.layers.Dense(100, activation=tf.nn.leaky_relu,
                                      kernel_initializer=tf.keras.initializers.VarianceScaling)(self.input_dot_state)

            xyztd = tf.keras.layers.concatenate([x, y, z, t, d])
            xyztd = tf.keras.layers.Dense(
                300, kernel_initializer=tf.keras.initializers.VarianceScaling)(xyztd)

            xyztd = tf.keras.layers.Dense(  # tf.nn.tanh
                self.output_channel, activation=None, trainable=trainable, kernel_initializer=tf.keras.initializers.VarianceScaling)(xyztd)
            self.l2_out = xyztd

            action_prob = tf.keras.layers.Softmax()(xyztd)

        params = tf.get_collection(tf.GraphKeys.GLOBAL_VARIABLES, scope=name)
        return action_prob, params

    def GetAction(self, state_image, target, state_pos, state_dir, state_dot):
        # action, l2 = self.sess.run([self.action_prob, self.l2_out], feed_dict={
        #     self.input_image_state: state_image})

        # action_prob = self.sess.run(self.pi, feed_dict={
        #     self.input_image_state: state_image})[0]
        action_prob, l2 = self.sess.run([self.pi, self.l2_out], feed_dict={
            self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_dot_state: state_dot,
            self.input_target_state: target})
        action_prob = action_prob[0]

        # old_action_prob = self.sess.run(self.old_pi, feed_dict={
        #     self.input_image_state: state_image})[0]
        print('------ get action info ------')
        print("l2:", np.array(l2))
        print(f'action_prob:{action_prob}')
        # print(f'old_action_prob:{old_action_prob}')
        # print("action:", np.array(action_prob))
        action = np.random.choice(range(5), 1, p=action_prob)
        return action, action_prob

    def ActionOneHot(self, action):
        action_onthot = np.zeros(5)
        action_onthot[action] = 1
        return action_onthot

    def get_v(self, state_image, target, state_pos, state_dir, state_dot):
        value = self.sess.run(self.critic_output_value,  feed_dict={
                              self.input_image_state: state_image, self.input_pos_state: state_pos, self.input_dir_state: state_dir, self.input_dot_state: state_dot,
                              self.input_target_state: target})[0, 0]

        print(f'value:{value}')
        return value

    def SaveModel(self, learn_step_counter, dir, name="test"):
        print("-----save controller model-----")
        self.saver.save(self.sess, dir + name +
                        '-c', global_step=learn_step_counter)

    def LoadModel(self, dir):
        print("-----load controller model-----")
        checkpoint = tf.train.get_checkpoint_state(dir)
        if checkpoint and checkpoint.model_checkpoint_path:
            self.saver.restore(self.sess, checkpoint.model_checkpoint_path)
            print("Successfully loaded:", checkpoint.model_checkpoint_path)
        else:
            print("Could not find old network weights")
