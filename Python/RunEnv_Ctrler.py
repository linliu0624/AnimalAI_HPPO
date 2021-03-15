#-----------------#
### 実行ファイル ###
#-----------------#
import mlagents
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfig, EngineConfigurationChannel
from mlagents_envs.environment import BaseEnv, UnityEnvironment
from mlagents_envs.registry import default_registry
from collections import deque
from math import floor
import numpy as np
from typing import NamedTuple, List, Dict, Tuple
import random
import time
import matplotlib.pyplot as plt
import GameManager as gm
import PPO_Controller as PPO_c
import PPO_MetaController as PPO_mc
###### game hyper-parameter config ######
# 環境の開き方, env_nameは開きたい環境の.exeを設定する
env_name = "C:/unity/exe/MyAnimalAI.exe"
# env_none-- unityのエディターで環境を開く
env_none = None
# ゲームのシミュレーション回数
MAX_EPISODES = 50000
BATCH_SIZE = 32
# モデルを保存する頻度(数字はエピソード)
SAVE_FREQ = 200
# 学習画像を保存するかないか
SAVE_INFO_IMG = True
# ランダムな部分目標を生成するかないか(下位層が独自で学習するときは，ランダムの部分目標が必要)
RANDOM_TARGET = False
# ゲームのスピード(早送りみたいなもの)
GAME_SPEED = 6

if __name__ == "__main__":
    ###### Set ml-agents environment ######
    # get channel from unity
    engine_configuration_channel = EngineConfigurationChannel()
    # Set game speed
    engine_configuration_channel.set_configuration_parameters(
        time_scale=GAME_SPEED)
    print("-----Choose environment-----")
    # set unity environment(file_name=開きたい環境を指定する)
    env = UnityEnvironment(file_name=env_none, side_channels=[
        engine_configuration_channel])
    print("-----environment creat success-----")
    # first reset environment
    env.reset()
    # get behavior name
    behavior_name = list(env.behavior_specs)[0]
    # Read and store the Behavior Specs of the Environment
    spec = env.behavior_specs[behavior_name]
    action_count = spec.discrete_action_branches
    # ###### get first step ######
    decision_steps, _ = env.get_steps(behavior_name)
    # レーザーセンターの出力のサイズをゲットする
    ray_obs_size = np.concatenate(
        (decision_steps.obs[1][0], decision_steps.obs[2][0])).shape[0]

    ###### class 實體化 ######
    ctrler = PPO_c.Controller(input_image_channel=6,
                              input_target_channel=3, input_dir_channel=3, input_pos_channel=3, input_dot_channel=1, output_channel=5)
    meta_ctrler = PPO_mc.MetaController(
        input_image_channel=6, input_dir_channel=3, input_pos_channel=3, input_ray_channel=ray_obs_size)
    # train_CTRLER, train_META_CTRLERそれぞれは下位層と上位層は学習させるかないか
    gameManager = gm.Game(train_CTRLER=False, train_META_CTRLER=True)

    ###### 変数の宣言 ######
    # 環境からの報酬を保存する配列
    all_ep_r = []
    # Criticからの報酬を保存する配列
    all_ep_inr = []
    # エピソードを記録する
    episodes_counter = 0
    # ステップを記録する
    steps_counter = 0

    ###### パラメータの読み込み ######
    # ctrler.LoadModel('saved_model/controller/')
    # meta_ctrler.LoadModel('saved_model/meta_controller/')

    ###### エピソードのループ ######
    for episode in range(0, MAX_EPISODES):
        print(f"\n=====New Episodes=====")
        # 環境を更新する
        env.step()
        # 報酬の初期化
        episode_reward = 0
        episode_inreward = 0
        # 入力画像, agent座標, agnet向き, agnet向きと部分目標の内積, レーザーセンサーの出力を保存する配列を宣言する
        buffer_s_image, buffer_s_pos, buffer_s_dir, buffer_s_dot, buffer_s_ray = [], [], [], [], []
        # 下位層の出力動作と環境からの報酬を保存する配列を宣言する
        buffer_a, buffer_exr = [], []
        # 上位層が出力した部分目標の座標, 部分目標座標をone hotに変換した情報とCriticからの報酬を保存する配列を宣言する
        buffer_target, buffer_onehot_target, buffer_inr = [], [], []

        ###### get first step ######
        # このステップの情報をunityからゲットする
        decision_steps, _ = env.get_steps(behavior_name)
        # 入力画像をゲットする
        state_image_t = decision_steps.obs[0][0, :, :, :]
        # 入力レーザーセンサーの情報をゲットする
        state_ray_obs_t = np.concatenate(
            (decision_steps.obs[1][0], decision_steps.obs[2][0]))
        # agentの座標, 向きとagnet向きと部分目標の内積をゲットする
        state_pos_t = decision_steps.obs[3][0][0:3]
        state_dir_t = decision_steps.obs[3][0][3:6]
        state_dot_t = [decision_steps.obs[3][0][6]]
        # ゲームはまだ終わらないから　False
        terminal = False

        ###### 2つのステップの画像を合併する ######
        for i in range(2):
            for j in range(3):
                image_set = gameManager.HoldImage(image=state_image_t[:, :, j])
        state_image_t = gameManager.HoldedImagesProcess('RGB')
        # ランダムな部分目標を生成する
        if RANDOM_TARGET:
            target_t = [random.uniform(-14, 14), 0, random.uniform(-14, 14)]
        ###### ステップのループ ######
        while True:
            print(
                f'-----Steps:{steps_counter} , Episodes:{episodes_counter}/{MAX_EPISODES}-----')
            # 上位層で部分目標を算出する
            if RANDOM_TARGET == False:
                # ここで算出する部分目標は1次元の配列です
                # フィールドは28*28なので，784のマスがある配列
                target_t, _ = meta_ctrler.GetTargetLabel(
                    [state_image_t], [state_pos_t], [state_dir_t], [state_ray_obs_t])
                target_t_onehot = meta_ctrler.TargetOneHot(target_t)
                # 上の部分を下の行で座標に変換する
                target_t = meta_ctrler.Target2Coordinate(target_t)

            # ステップごとにランダムな部分目標を生成する
            # if RANDOM_TARGET:
            #     target_t = [random.uniform(-14, 14), 0, random.uniform(-14, 14)]

            # 下位層で動作を算出する(a_probは各動作の確率)
            action_t, a_prob = ctrler.GetAction([state_image_t], [target_t], [
                                                state_pos_t], [state_dir_t], [state_dot_t])

            # print(f'action_t:{action_t}')
            # 動作と部分目標を1つの配列に合併する
            action_target = np.concatenate((action_t, target_t))
            # 動作と部分目標座標をunityに送る(部分目標を送る理由は環境で部分目標を描画したいからだ)
            env.set_actions(behavior_name, np.array([action_target]))
            # env.set_actions(behavior_name, np.array([action_t]))
            # 動作をonehotにする
            action_t = ctrler.ActionOneHot(action_t)
            # 環境を更新する
            env.step()

            ###### 新たなステップの情報をゲットする ######
            decision_steps, terminal_steps = env.get_steps(behavior_name)
            # エピソードが途中の場合
            for agent_id_decisions in decision_steps:
                # step t+1の画像をゲットする
                state_t1 = decision_steps.obs[0][0, :, :, :]
                # step tの環境からの報酬をゲットする
                ex_reward_t = decision_steps.reward
                # step tはまだ終わってない
                terminal = False
                # step t+1のレーザーセンサー情報, agent座標,向きと内積をゲットする
                state_ray_obs_t1 = np.concatenate(
                    (decision_steps.obs[1][0], decision_steps.obs[2][0]))
                state_pos_t1 = decision_steps.obs[3][0][0:3]
                state_dir_t1 = decision_steps.obs[3][0][3:6]
                state_dot_t1 = [decision_steps.obs[3][0][6]]

            # このエピソードの最後のステップの場合(最後かどうかはunityで判断する)
            for agent_id_terminated in terminal_steps:
                # teminal以外は同上
                # obs step t+1
                state_t1 = terminal_steps.obs[0][0, :, :, :]
                # reward t
                ex_reward_t = terminal_steps.reward
                # terminal t(ゲームが終わったのでTrue)
                terminal = True
                # state info t1
                state_ray_obs_t1 = np.concatenate(
                    (terminal_steps.obs[1][0], terminal_steps.obs[2][0]))
                state_pos_t1 = terminal_steps.obs[3][0][0:3]
                state_dir_t1 = terminal_steps.obs[3][0][3:6]
                state_dot_t1 = [terminal_steps.obs[3][0][6]]

            ###### step t と t+1の画像をstackに保存する ######
            for j in range(3):
                image_set = gameManager.HoldImage(image=state_t1[:, :, j])
            state_t1 = gameManager.HoldedImagesProcess('RGB')

            ###### 下位層の動作をCriticで報酬を計算する ######
            in_reward_t, new_dis = gameManager.Critic(
                state_pos_t, state_pos_t1, state_dot_t, state_dot_t1, target_t, np.argmax(action_t))

            ###### 情報をbufferに保存する ######
            buffer_s_image.append(state_image_t)
            buffer_s_pos.append(state_pos_t)
            buffer_s_dir.append(state_dir_t)
            buffer_s_dot.append(state_dot_t)
            buffer_s_ray.append(state_ray_obs_t)
            buffer_target.append(target_t)
            buffer_onehot_target.append(target_t_onehot)

            buffer_a.append(action_t)

            buffer_exr.append(ex_reward_t)
            buffer_inr.append(in_reward_t)

            ###### データを更新する ######
            episode_reward += ex_reward_t
            episode_inreward += in_reward_t
            steps_counter += 1

            state_image_t = state_t1
            state_pos_t = state_pos_t1
            state_dir_t = state_dir_t1
            state_dot_t = state_dot_t1
            state_ray_obs_t = state_ray_obs_t1

            # ランダムの部分目標を更新する
            if new_dis < 1 and RANDOM_TARGET:
                target_t = [
                    random.uniform(-14, 14), 0, random.uniform(-14, 14)]

            # print(f'action prob:{a_prob}')
            print('------ game info ------')
            print(f'action:{np.argmax(action_t)}')
            print(f'ew reward:{ex_reward_t}')
            print(f'in reward:{in_reward_t}')

            ###### 学習 ######
            if len(buffer_s_image) % BATCH_SIZE == 0 or terminal:
                tmp_inr = []
                tmp_exr = []
                ### 下位層 ###
                # 下位層のcritic部の計算
                c_value_s1 = ctrler.get_v([state_t1], [target_t], [
                                          state_pos_t1], [state_dir_t1], [state_dot_t1])
                c_discounted_r = []
                for r in buffer_inr[::-1]:
                    # TD errorのターゲット値の計算
                    c_value_next = r + ctrler.gamma*c_value_s1
                    c_discounted_r.append(c_value_next)
                    tmp_inr.append(r)
                c_discounted_r.reverse()
                c_discounted_r = np.array(c_discounted_r)[:, np.newaxis]

                ### 上位層 ###
                for r in buffer_exr[::-1]:
                    tmp_exr.append(r)
                 # 上位層のcritic部の計算
                mc_value_s1 = meta_ctrler.get_v(
                    [state_t1], [state_pos_t1], [state_dir_t1], [state_ray_obs_t1])
                mc_discounted_r = []
                for i in range(len(tmp_inr)):
                    mixr = tmp_exr[i]
                    # TD errorのターゲット値の計算
                    mc_value_next = mixr + meta_ctrler.gamma*mc_value_s1
                    mc_discounted_r.append(mc_value_next)
                mc_discounted_r.reverse()
                # ニューラルネットワークを更新する
                if gameManager.train_controller:
                    ctrler.update(buffer_s_image, buffer_target,
                                  buffer_s_pos, buffer_s_dir, buffer_s_dot, buffer_a, c_discounted_r)
                if gameManager.train_meta_controller:
                    meta_ctrler.update(
                        buffer_s_image, buffer_s_pos, buffer_s_dir, buffer_s_ray, buffer_onehot_target, mc_discounted_r)
                # reset buffer
                buffer_s_image, buffer_s_pos, buffer_s_dir, buffer_s_dot, buffer_s_ray = [], [], [], [], []
                buffer_a, buffer_exr = [], []
                buffer_target, buffer_inr = [], []
                buffer_onehot_target = []
            ###### エピソードが終わったら ######
            if terminal:

                print(f'episode_exreward:{episode_reward}')
                print(f'episode_inreward:{episode_inreward}')

                ###### 資訊輸出與可視化 ######
                if len(all_ep_inr) == 0:
                    all_ep_inr.append(episode_inreward)
                else:
                    all_ep_inr.append(all_ep_inr[-1]*0.9+episode_inreward*0.1)

                if len(all_ep_r) == 0:
                    all_ep_r.append(episode_reward)
                else:
                    all_ep_r.append(all_ep_r[-1]*0.9+episode_reward*0.1)
                # 学習した画像を保存する(報酬の推移図)
                if SAVE_INFO_IMG == True:
                    if episodes_counter < 2000:
                        if episodes_counter % 5 == 0 and episodes_counter != 0:
                            gameManager.ShowGraph(
                                all_ep_r, dir='image/', name='ex_reward')
                            gameManager.ShowGraph(
                                all_ep_inr, dir='image/', name='in_reward')
                    else:
                        if episodes_counter % 50 == 0:
                            gameManager.ShowGraph(
                                all_ep_r, dir='image/', name='ex_reward')
                            gameManager.ShowGraph(
                                all_ep_inr, dir='image/', name='in_reward')
                    ###### パラメータとモデルを保存する ######
                    if episodes_counter % SAVE_FREQ == 0 and episodes_counter != 0:
                        ctrler.SaveModel(
                            episodes_counter, 'saved_model/controller/', 'add')
                        meta_ctrler.SaveModel(
                            episodes_counter, 'saved_model/meta_controller/', 'blff')

                    episodes_counter += 1
                break
    # ゲームを閉じる
    env.close()
