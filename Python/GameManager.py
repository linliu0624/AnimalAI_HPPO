### 実行ファイルで使う関数 ###
import matplotlib.pyplot as plt
import random
from typing import NamedTuple, List, Dict, Tuple
from collections import deque
import numpy as np
np.set_printoptions(threshold=np.inf)


class Game():
    def __init__(self, train_META_CTRLER=False, train_CTRLER=False):
        self.image_holder = deque()
        self.label_holder = deque()
        self.game_steps = 0
        self.train_meta_controller = train_META_CTRLER
        self.train_controller = train_CTRLER

    def Critic(self, old_pos, new_pos, old_dot, new_dot, target, action):
        ''' agentと部分目標の距離はここで計算するが
            内積はunity上で計算する '''
        in_reward = 0
        old_dis = 0
        new_dis = 0

        ###### 新旧座標の処理 ######
        # +14する理由は環境フィールドの中心の座標は(0,0,0)だが
        # 範囲は(-14 ~ 14)なので，負数の計算はしたくないので，全部正数にする
        old_pos2D = np.array([old_pos[0]+14, old_pos[2]+14])
        new_pos2D = np.array([new_pos[0]+14, new_pos[2]+14])
        target2D = np.array([target[0]+14, target[2]+14])

        ###### 距離の計算 ######
        old_dis = np.sqrt(np.square(old_pos2D[0]-target2D[0]) +
                          np.square(old_pos2D[1]-target2D[1]))
        new_dis = np.sqrt(np.square(new_pos2D[0]-target2D[0]) +
                          np.square(new_pos2D[1]-target2D[1]))
        old_dis = np.around(old_dis, 4)
        new_dis = np.around(new_dis, 4)
        ###### criticの核心, 下位層動作の報酬の計算 ######
        # 動作は停止だと
        if action == 0:
            in_reward = -0.05
        # 前後移動の場合
        elif action == 1 or action == 2:
            # 部分目標に離れたら
            if old_dis < new_dis:
                in_reward += -0.15
            # 内積は0.95より大きい時に，距離を縮めると
            elif old_dis > new_dis:
                if old_dot[0] > 0.95:
                    in_reward += 0.3
        # 時計回りと逆時計回りの時
        else:
            if old_dot < new_dot:
                in_reward += 0.1
            elif old_dot > new_dot:
                in_reward += -0.15
            else:
                in_reward += 0

        # 部分目標にタッチしたら
        if new_dis < 1.0:
            in_reward += 1

        print('------ intrinsic reward ------')
        print(f'target:{target2D}')
        print(f'old_pos:{old_pos2D}')
        print(f'new_pos:{new_pos2D}')
        print(f'old_dot:{old_dot}')
        print(f'new_dot:{new_dot}')
        print("old_dis:", old_dis)
        print("new_dis:", new_dis)

        return in_reward, new_dis

    ### 以下2つのメソッドは複数のステップの画像を合併するところ ###
    def HoldImage(self, image, hold_num=6):
        self.image_holder.append(image)
        if len(self.image_holder) > hold_num:
            self.image_holder.popleft()
        # print(np.array(self.image_holder).shape)
        return self.image_holder

    def HoldedImagesProcess(self, image_type='gray'):
        image_set = self.image_holder
        if image_type == 'gray':
            image_set = np.stack(
                (image_set[0], image_set[1], image_set[2], image_set[3]), axis=3)
            image_set = np.squeeze(image_set)
        else:
            image_set = np.stack(
                (image_set[0], image_set[1], image_set[2], image_set[3], image_set[4], image_set[5]), axis=2)
            image_set = np.squeeze(image_set)
        # print("image set shape:", image_set.shape)
        return image_set

    # 報酬の推移図などの画像を生成するメソッド
    def ShowGraph(self, data, x_label='episodes', y_label='reward', dir='image/', name='reward'):
        print('-----save image-----')
        ax = plt.subplots()
        plt.plot(np.arange(len(data)), data)
        plt.xlabel(x_label)
        plt.ylabel(y_label)
        plt.savefig(dir+name)
        plt.close('all')
