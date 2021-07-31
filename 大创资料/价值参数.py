import pandas as pd
import numpy as np
import os

excel_path = '总数据1.xlsx'
data = pd.read_excel(excel_path, sheet_name="舒适度约束")
data = np.array(data)
t0 = 0
W = 3.5

L_M = L_Fd = L_Ld = L_Fo = L_Lo = 4.5  # 所有车辆的长度
W_M = W_Fd = W_Ld = W_Fo = W_Lo = 1.8  # 所有车辆的宽度
gamma_x = gamma_y = 0.5  # x和y方向的舒适度权重
data_value = np.array(['tc', 'xc', 'j', 'x_cl', 'tc'])


def X_M(t, tc, xc):
    # 初始条件x后数字代表求导次数，_后面的数字代表t0和tc状态
    x0_0 = x2_0 = x2_c = 0
    x1_0 = 30 / 3.6
    x0_c = xc
    x1_c = 40 / 3.6
    T = tc - t0

    # 系数
    h_a = x0_c - x0_0
    a0 = x0_0
    a1 = x1_0
    a2 = 0.5 * x2_0
    a3 = (20 * h_a - (8 * x1_c + 12 * x1_0) * T - (3 * x2_0 - x2_c) / (T ** 2)) / (2 * T ** 3)
    a4 = (-30 * h_a + (14 * x1_c + 16 * x1_0) * T + (3 * x2_0 - 2 * x2_c) / (T ** 2)) / (2 * T ** 4)
    a5 = (12 * h_a - 6 * (x1_c + x1_0) * T + (x2_c - x2_0) / (T ** 2)) / (2 * T ** 5)

    return a0 + a1 * t + a2 * t ** 2 + a3 * t ** 3 + a4 * t ** 4 + a5 * t ** 5


def X_M1(t, tc, xc):
    # 初始条件x后数字代表求导次数，_后面的数字代表t0和tc状态
    x0_0 = x2_0 = x2_c = 0
    x1_0 = 30 / 3.6
    x0_c = xc
    x1_c = 40 / 3.6
    T = tc - t0

    # 系数
    h_a = x0_c - x0_0
    a1 = x1_0
    a2 = 0.5 * x2_0
    a3 = (20 * h_a - (8 * x1_c + 12 * x1_0) * T - (3 * x2_0 - x2_c) / (T ** 2)) / (2 * T ** 3)
    a4 = (-30 * h_a + (14 * x1_c + 16 * x1_0) * T + (3 * x2_0 - 2 * x2_c) / (T ** 2)) / (2 * T ** 4)
    a5 = (12 * h_a - 6 * (x1_c + x1_0) * T + (x2_c - x2_0) / (T ** 2)) / (2 * T ** 5)

    return a1 + 2 * a2 * t + 3 * a3 * t ** 2 + 4 * a4 * t ** 3 + 5 * a5 * t ** 4


def X_M2(t, tc, xc):
    # 初始条件x后数字代表求导次数，_后面的数字代表t0和tc状态
    x0_0 = 0
    x1_0 = 30 / 3.6
    x2_0 = 0
    x0_c = xc
    x1_c = 40 / 3.6
    x2_c = 0
    T = tc - t0

    # 系数
    h_a = x0_c - x0_0
    a2 = 0.5 * x2_0
    a3 = (20 * h_a - (8 * x1_c + 12 * x1_0) * T - (3 * x2_0 - x2_c) / (T ** 2)) / (2 * T ** 3)
    a4 = (-30 * h_a + (14 * x1_c + 16 * x1_0) * T + (3 * x2_0 - 2 * x2_c) / (T ** 2)) / (2 * T ** 4)
    a5 = (12 * h_a - 6 * (x1_c + x1_0) * T + (x2_c - x2_0) / (T ** 2)) / (2 * T ** 5)

    return 2 * a2 + 6 * a3 * t + 12 * a4 * t ** 2 + 20 * a5 * t ** 3


def X_M3(t, tc, xc):
    # 初始条件x后数字代表求导次数，_后面的数字代表t0和tc状态
    x0_0 = 0
    x1_0 = 30 / 3.6
    x2_0 = 0
    x0_c = xc
    x1_c = 40 / 3.6
    x2_c = 0
    T = tc - t0

    # 系数
    h_a = x0_c - x0_0
    a3 = (20 * h_a - (8 * x1_c + 12 * x1_0) * T - (3 * x2_0 - x2_c) / (T ** 2)) / (2 * T ** 3)
    a4 = (-30 * h_a + (14 * x1_c + 16 * x1_0) * T + (3 * x2_0 - 2 * x2_c) / (T ** 2)) / (2 * T ** 4)
    a5 = (12 * h_a - 6 * (x1_c + x1_0) * T + (x2_c - x2_0) / (T ** 2)) / (2 * T ** 5)

    return 6 * a3 + 24 * a4 * t + 60 * a5 * t ** 2


def X_M4(t, tc, xc):
    # 初始条件x后数字代表求导次数，_后面的数字代表t0和tc状态
    x0_0 = 0
    x1_0 = 30 / 3.6
    x2_0 = 0
    x0_c = xc
    x1_c = 40 / 3.6
    x2_c = 0
    T = tc - t0

    # 系数
    h_a = x0_c - x0_0
    a4 = (-30 * h_a + (14 * x1_c + 16 * x1_0) * T + (3 * x2_0 - 2 * x2_c) / (T ** 2)) / (2 * T ** 4)
    a5 = (12 * h_a - 6 * (x1_c + x1_0) * T + (x2_c - x2_0) / (T ** 2)) / (2 * T ** 5)

    return 24 * a4 + 120 * a5 * t


def X_M5(t, tc, xc):
    # 初始条件x后数字代表求导次数，_后面的数字代表t0和tc状态
    x0_0 = 0
    x1_0 = 30 / 3.6
    x2_0 = 0
    x0_c = xc
    x1_c = 40 / 3.6
    x2_c = 0
    T = tc - t0

    # 系数
    h_a = x0_c - x0_0
    a5 = (12 * h_a - 6 * (x1_c + x1_0) * T + (x2_c - x2_0) / (T ** 2)) / (2 * T ** 5)

    return 120 * a5


def Y_M(t, tc):
    y0_0 = 0
    y1_0 = 0
    y2_0 = 0
    y0_c = W
    y1_c = 0
    y2_c = 0

    # 系数
    T = tc - t0
    h_b = y0_c - y0_0
    b0 = y0_0
    b1 = y1_0
    b2 = 0.5 * y2_0
    b3 = (20 * h_b - (8 * y1_c + 12 * y1_0) * T - (3 * y2_0 - y2_c) / (T ** 2)) / (2 * T ** 3)
    b4 = (-30 * h_b + (14 * y1_c + 16 * y1_0) * T + (3 * y2_0 - 2 * y2_c) / (T ** 2)) / (2 * T ** 4)
    b5 = (12 * h_b - 6 * (y1_c + y1_0) * T + (y2_c - y2_0) / (T ** 2)) / (2 * T ** 5)

    return b0 + b1 * t + b2 * t ** 2 + b3 * t ** 3 + b4 * t ** 4 + b5 * t ** 5


def Y_M1(t, tc):
    y0_0 = 0
    y1_0 = 0
    y2_0 = 0
    y0_c = W
    y1_c = 0
    y2_c = 0

    # 系数
    T = tc - t0
    h_b = y0_c - y0_0
    b1 = y1_0
    b2 = 0.5 * y2_0
    b3 = (20 * h_b - (8 * y1_c + 12 * y1_0) * T - (3 * y2_0 - y2_c) / (T ** 2)) / (2 * T ** 3)
    b4 = (-30 * h_b + (14 * y1_c + 16 * y1_0) * T + (3 * y2_0 - 2 * y2_c) / (T ** 2)) / (2 * T ** 4)
    b5 = (12 * h_b - 6 * (y1_c + y1_0) * T + (y2_c - y2_0) / (T ** 2)) / (2 * T ** 5)

    return b1 + 2 * b2 * t + 3 * b3 * t ** 2 + 4 * b4 * t ** 3 + 5 * b5 * t ** 4


def Y_M2(t, tc):
    y0_0 = 0
    y1_0 = 0
    y2_0 = 0
    y0_c = W
    y1_c = 0
    y2_c = 0

    # 系数
    T = tc - t0
    h_b = y0_c - y0_0
    b2 = 0.5 * y2_0
    b3 = (20 * h_b - (8 * y1_c + 12 * y1_0) * T - (3 * y2_0 - y2_c) / (T ** 2)) / (2 * T ** 3)
    b4 = (-30 * h_b + (14 * y1_c + 16 * y1_0) * T + (3 * y2_0 - 2 * y2_c) / (T ** 2)) / (2 * T ** 4)
    b5 = (12 * h_b - 6 * (y1_c + y1_0) * T + (y2_c - y2_0) / (T ** 2)) / (2 * T ** 5)

    return 2 * b2 + 6 * b3 * t + 12 * b4 * t ** 2 + 20 * b5 * t ** 3


def Y_M3(t, tc):
    y0_0 = 0
    y1_0 = 0
    y2_0 = 0
    y0_c = W
    y1_c = 0
    y2_c = 0

    # 系数
    T = tc - t0
    h_b = y0_c - y0_0
    b3 = (20 * h_b - (8 * y1_c + 12 * y1_0) * T - (3 * y2_0 - y2_c) / (T ** 2)) / (2 * T ** 3)
    b4 = (-30 * h_b + (14 * y1_c + 16 * y1_0) * T + (3 * y2_0 - 2 * y2_c) / (T ** 2)) / (2 * T ** 4)
    b5 = (12 * h_b - 6 * (y1_c + y1_0) * T + (y2_c - y2_0) / (T ** 2)) / (2 * T ** 5)

    return 6 * b3 + 24 * b4 * t + 60 * b5 * t ** 2


def Y_M4(t, tc):
    y0_0 = 0
    y1_0 = 0
    y2_0 = 0
    y0_c = W
    y1_c = 0
    y2_c = 0

    # 系数
    T = tc - t0
    h_b = y0_c - y0_0
    b4 = (-30 * h_b + (14 * y1_c + 16 * y1_0) * T + (3 * y2_0 - 2 * y2_c) / (T ** 2)) / (2 * T ** 4)
    b5 = (12 * h_b - 6 * (y1_c + y1_0) * T + (y2_c - y2_0) / (T ** 2)) / (2 * T ** 5)

    return 24 * b4 + 120 * b5 * t


def Y_M5(t, tc):
    y0_0 = 0
    y1_0 = 0
    y2_0 = 0
    y0_c = W
    y1_c = 0
    y2_c = 0

    # 系数
    T = tc - t0
    h_b = y0_c - y0_0
    b5 = (12 * h_b - 6 * (y1_c + y1_0) * T + (y2_c - y2_0) / (T ** 2)) / (2 * T ** 5)

    return 120 * b5


for line in np.arange(0, len(data), 1):
    '''
    x方向
    a3 = data[line][7], a4 = data[line][8], a5 = data[line][9], tc = data[line][1], t0 = 0
    '''
    delta_x = data[line][8] ** 2 - 2.5 * data[line][7] * data[line][9]
    if delta_x <= 0:
        print("加加速度与x轴无交点，且加速度方向未变")
        # situation = 1  # 积分部分曲线未跨越x轴
        j_x = abs(X_M2(data[line][1], data[line][1], data[line][3]) - X_M2(0, data[line][1], data[line][3]))
    else:
        if data[line][9] == 0:
            j_x = abs((X_M3(data[line][1], data[line][1], data[line][3]) - X_M3(0, data[line][1], data[line][3])) *
                      data[line][1])
        else:
            root_1, root_2 = sorted(
                [(-data[line][8] - delta_x) / (6 * data[line][9]), (-data[line][8] + delta_x) / (6 * data[line][9])])
            if data[line][1] <= root_1 or root_1 <= 0 <= data[line][1] <= root_2 or root_2 <= 0:
                # situation = 1
                j_x = abs(
                    X_M2(data[line][1], data[line][1], data[line][3]) - X_M2(0, data[line][1], data[line][3]))
            elif 0 <= root_1 <= data[line][1]:
                # situation = 2  # 曲线跨越一次x轴
                j_x = abs(X_M2(root_1, data[line][1], data[line][3]) - X_M2(0, data[line][1], data[line][3])) + abs(
                    X_M2(data[line][1], data[line][1], data[line][3]) - X_M2(root_1, data[line][1], data[line][3]))
            elif 0 <= root_2 <= data[line][1]:
                # situation = 2  # 曲线跨越一次x轴
                j_x = abs(X_M2(root_2, data[line][1], data[line][3]) - X_M2(0, data[line][1], data[line][3])) + abs(
                    X_M2(data[line][1], data[line][1], data[line][3]) - X_M2(root_2, data[line][1], data[line][3]))
            elif 0 <= root_1 <= root_2 <= data[line][1]:
                # situation = 3  # 曲线跨越二次x轴
                j_x = abs(
                    X_M2(data[line][1], data[line][1], data[line][3]) - X_M2(0, data[line][1],
                                                                             data[line][3])) + 2 * abs(
                    2 * (X_M2(root_2, data[line][1], data[line][3]) - X_M2(root_1, data[line][1], data[line][3])))
            else:
                print("x方向舒适度出现想象之外的情况")
                j_x = float('inf')
                os.system("pause")

    '''
    y方向
    b3 = data[line][13], b4 = data[line][14], b5 = data[line][15], tc = data[line][1], t0 = 0
    '''
    delta_y = data[line][14] ** 2 - 2.5 * data[line][13] * data[line][15]
    if delta_y <= 0:
        print("加加速度与x轴无交点，且加速度方向未变")
        # situation = 1  # 积分部分曲线未跨越x轴
        j_y = abs(Y_M2(data[line][1], data[line][1]) - Y_M2(0, data[line][1]))
    else:
        if data[line][9] == 0:
            j_y = abs((Y_M3(data[line][1], data[line][1]) - Y_M3(0, data[line][1])) * data[line][1])
        else:
            root_3, root_4 = sorted(
                [(-data[line][14] - delta_y) / (6 * data[line][15]),
                 (-data[line][14] + delta_y) / (6 * data[line][15])])
            if data[line][1] <= root_3 or root_3 <= 0 <= data[line][1] <= root_4 or root_4 <= 0:
                # situation = 1
                j_y = abs(Y_M2(data[line][1], data[line][1]) - Y_M2(0, data[line][1]))
            elif 0 <= root_3 <= data[line][1]:
                # situation = 2  # 曲线跨越一次x轴
                j_y = abs(Y_M2(root_3, data[line][1]) - Y_M2(0, data[line][1])) + abs(
                    Y_M2(data[line][1], data[line][1]) - Y_M2(root_3, data[line][1]))
            elif 0 <= root_4 <= data[line][1]:
                # situation = 2  # 曲线跨越一次x轴
                j_y = abs(Y_M2(root_4, data[line][1]) - Y_M2(0, data[line][1])) + abs(
                    Y_M2(data[line][1], data[line][1]) - Y_M2(root_4, data[line][1]))
            elif 0 <= root_3 <= root_4 <= data[line][1]:
                # situation = 3  # 曲线跨越二次x轴
                j_y = abs(Y_M2(data[line][1], data[line][1]) - Y_M2(0, data[line][1])) + 2 * abs(
                    2 * (Y_M2(root_4, data[line][1]) - Y_M2(root_3, data[line][1])))
            else:
                print("y方向舒适度出现想象之外的情况")
                j_y = float('inf')
                os.system("pause")

    j = j_x * gamma_x + j_y * gamma_y

    '''下面是x_cl的计算'''
    x_cl = None
    for t in np.arange(0, data[line][1] + 0.1, 0.1):
        if Y_M(t, data[line][1]) + 0.5 * (L_M ** 2 + W_M ** 2) ** 0.5 * Y_M1(t, data[line][1]) / (
                Y_M1(t, data[line][1]) ** 2 + X_M1(t, data[line][1], data[line][3]) ** 2) ** 0.5 >= W / 2:
            x1 = X_M(t, data[line][1], data[line][3])
            if Y_M(t, data[line][1]) - 0.5 * (L_M ** 2 + W_M ** 2) ** 0.5 * Y_M1(t, data[line][1]) / (
                    Y_M1(t, data[line][1]) ** 2 + X_M1(t, data[line][1], data[line][3]) ** 2) ** 0.5 >= W / 2:
                x2 = X_M(t, data[line][1], data[line][3])
                x_cl = x2 - x1
                break
    if x_cl is None:
        print("第{0}行，tc={1}，xc={2},舒适度出现错误".format(line, data[line][1], data[line][3]))

    data_value_i = np.array([data[line][1], data[line][3], j, x_cl, data[line][1]])
    data_value = np.vstack((data_value, data_value_i))

writer = pd.ExcelWriter('价值参数.xlsx')
df = pd.DataFrame(data_value)
df.to_excel(writer, "价值参数", index=0, header=0)
writer.save()
