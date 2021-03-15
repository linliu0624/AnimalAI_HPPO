import tensorflow as tf
from tensorflow.contrib.distributions import Normal
import numpy as np
import matplotlib.pyplot as plt

GAMMA = 0.9
A_LR = 1e-7
C_LR = 2e-7
A_UPDATE_STEPS = 10
C_UPDATE_STEPS = 10
CLIP_EPSILON=0.2


class MetaController(object):
    def __init__(self, input_channel, output_channel, gamma=GAMMA):
        self.sess = tf.Session()

        self.input_channel = input_channel
        self.output_channel = output_channel
        self.gamma = gamma

        self.input_image_state = tf.placeholder(
            tf.float32, [None, 90, 90, self.input_channel], 'mc_input_label_state')
        self.input_label_state = tf.placeholder(
            tf.float32, [None, 14], 'mc_input_label_state')
        self.target_label = tf.placeholder(
            tf.float32, [None, self.output_channel], 'mc_target')
        self.tfadv = tf.placeholder(tf.float32, [None, 1], 'mc_advantage')

        # critic
        self.discounted_reward = tf.placeholder(
            tf.float32, [None, 1], 'mc_discounted_r')
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
            
            self.critic_output_value = tf.keras.layers.Dense(1)(x)

            self.advantage = self.discounted_reward - self.critic_output_value
            self.critic_loss = tf.reduce_mean(tf.square(self.advantage))
            self.critic_train_op = tf.train.AdamOptimizer(
                C_LR).minimize(self.critic_loss)

        # actor
        self.pi, pi_params = self._build_anet('mc_pi', trainable=True)
        self.old_pi, oldpi_params = self._build_anet(
            'mc_oldpi', trainable=False)

        with tf.variable_scope('mc_update_oldpi'):
            self.update_oldpi_op = [oldp.assign(
                p) for p, oldp in zip(pi_params, oldpi_params)]
            
        with tf.variable_scope('sample_op'):
            self.sample_op = tf.squeeze(self.pi.sample(1), axis=0)
            
        with tf.variable_scope('mc_loss'):
            with tf.variable_scope('mc_surrogate'):
                # self.ratio = tf.exp(
                #     pi.log_prob(self.target_label) - old_pi.log_prob(self.target_label))
                self.ratio = self.pi.prob(self.target_label) / \
                    (self.old_pi.prob(self.target_label) + 1e-5)

                self.surr = self.ratio * self.tfadv
                self.actor_loss = -tf.reduce_mean(tf.minimum(
                    self.surr,
                    tf.clip_by_value(self.ratio, 1.0-CLIP_EPSILON, 1.0+CLIP_EPSILON)*self.tfadv))

        with tf.variable_scope('mc_atrain'):
            self.atrain_op = tf.train.AdamOptimizer(
                A_LR).minimize(self.actor_loss)

        # tf.summary.FileWriter("log/", self.sess.graph)

        self.sess.run(tf.global_variables_initializer())
        self.saver = tf.compat.v1.train.Saver(max_to_keep=5)

    def update(self, s, a, r):
        # print("update_oldpi_op")
        self.sess.run(self.update_oldpi_op)
        # print("adv")
        adv = self.sess.run(self.advantage, feed_dict={
                            self.input_image_state: s, self.discounted_reward: r})
        # adv = (adv - adv.mean())/(adv.std()+1e-6)     # sometimes helpful
        # update actor
        [self.sess.run(self.atrain_op, {
            self.input_image_state: s, self.target_label: a, self.tfadv: adv}) for _ in range(A_UPDATE_STEPS)]
        
        loss, ratio, surr = self.sess.run([self.actor_loss, self.ratio, self.surr], {
            self.input_image_state: s, self.target_label: a, self.tfadv: adv})
        print('------ update meta controller info ------')
        print('mc_actor loss:', loss)
        print('mc_adv:',np.mean(adv))
        print('mc_surr:', np.mean(surr))
        # update critic
        # print("critic_train_op")
        [self.sess.run(self.critic_train_op, {self.input_image_state: s, self.discounted_reward: r})
         for _ in range(C_UPDATE_STEPS)]

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
            
            mu = 0.5*tf.keras.layers.Dense(\
                self.output_channel, activation=tf.nn.sigmoid, trainable=trainable)(x) #tf.nn.tanh
            sigma = tf.keras.layers.Dense(\
                self.output_channel, activation=tf.nn.sigmoid, trainable=trainable)(x) #tf.nn.softplus
            norm_dist = Normal(loc=mu, scale=sigma)
            
            self.mu = mu
            self.sigma = sigma

        params = tf.get_collection(tf.GraphKeys.GLOBAL_VARIABLES, scope=name)
        return norm_dist, params

    def GetTargetLabel(self, state_image):
        target, mu, sigma = self.sess.run([self.sample_op, self.mu, self.sigma], feed_dict={
            self.input_image_state: state_image})
        target = target[0]
        print('------ get target label info ------')
        print("mu:", mu)
        print("sigma:", sigma)
        # action_prob = self.sess.run(self.pi, feed_dict={
        #     self.input_image_state: state_image})[0]
        print(f'target:{target}')
        
        # print(f'old_action_prob:{old_action_prob}')
        # print("action:", np.array(action_prob))
        return np.clip(target, 0, 1)

    def get_v(self, s):
        value = self.sess.run(self.critic_output_value,  feed_dict={ self.input_image_state: s})[0, 0]
        print(f'mc value:{value}')
        return value

    def SaveModel(self, learn_step_counter, dir, name="test"):
        print("-----save controller model-----")
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