import numpy as np
from sympy import atan, cos, sin
import pandas as pd

# 时间
t0 = 0
W = 3.5


def X_M(t, tc, xc):
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
    a0 = x0_0
    a1 = x1_0
    a2 = 0.5 * x2_0
    a3 = (20 * h_a - (8 * x1_c + 12 * x1_0) * T - (3 * x2_0 - x2_c) / (T ** 2)) / (2 * T ** 3)
    a4 = (-30 * h_a + (14 * x1_c + 16 * x1_0) * T + (3 * x2_0 - 2 * x2_c) / (T ** 2)) / (2 * T ** 4)
    a5 = (12 * h_a - 6 * (x1_c + x1_0) * T + (x2_c - x2_0) / (T ** 2)) / (2 * T ** 5)

    return a0 + a1 * t + a2 * t ** 2 + a3 * t ** 3 + a4 * t ** 4 + a5 * t ** 5


def X_M1(t, tc, xc):
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


# Fd车辆的运行轨迹函数
def X_Fd(t):
    return -20 + 40 / 3.6 * t


def Y_Fd(t):
    return W


# Ld车辆的运行轨迹函数
def X_Ld(t):
    return 15 + 40 / 3.6 * t


def Y_Ld(t):
    return W


# Fo车辆的运行轨迹函数
def X_Fo(t):
    return -25 + 30 / 3.6 * t


def Y_Fo(t):
    return 0


# Fo车辆的运行轨迹函数
def X_Lo(t):
    return 20 + 30 / 3.6 * t


def Y_Lo(t):
    return 0


def s(mx, my, m_yaw_angle, ml, mw, tx, ty, t_yaw_angle, tl, tw):
    m_squar = np.array([[mx - (ml / 2 * cos(m_yaw_angle) - mw / 2 * sin(m_yaw_angle)),
                         my - (mw / 2 * cos(m_yaw_angle) + ml / 2 * sin(m_yaw_angle))],
                        [mx + (mw / 2 * sin(m_yaw_angle) + ml / 2 * cos(m_yaw_angle)),
                         my + (ml / 2 * sin(m_yaw_angle) - mw / 2 * cos(m_yaw_angle))],
                        [mx + (ml / 2 * cos(m_yaw_angle) - mw / 2 * sin(m_yaw_angle)),
                         my + (ml / 2 * sin(m_yaw_angle) + mw / 2 * cos(m_yaw_angle))],
                        [mx - (ml / 2 * cos(m_yaw_angle) + mw / 2 * sin(m_yaw_angle)),
                         my + (mw / 2 * cos(m_yaw_angle) - ml / 2 * sin(m_yaw_angle))]])
    t_squar = np.array([[tx - (tl / 2 * cos(t_yaw_angle) - tw / 2 * sin(t_yaw_angle)),
                         ty - (tw / 2 * cos(t_yaw_angle) + tl / 2 * sin(t_yaw_angle))],
                        [tx + (tw / 2 * sin(t_yaw_angle) + tl / 2 * cos(t_yaw_angle)),
                         ty + (tl / 2 * sin(t_yaw_angle) - tw / 2 * cos(t_yaw_angle))],
                        [tx + (tl / 2 * cos(t_yaw_angle) - tw / 2 * sin(t_yaw_angle)),
                         ty + (tl / 2 * sin(t_yaw_angle) + tw / 2 * cos(t_yaw_angle))],
                        [tx - (tl / 2 * cos(t_yaw_angle) + tw / 2 * sin(t_yaw_angle)),
                         ty + (tw / 2 * cos(t_yaw_angle) - tl / 2 * sin(t_yaw_angle))]])
    t1t2 = t_squar[1] - t_squar[0]
    t1t4 = t_squar[3] - t_squar[0]
    t3t2 = t_squar[1] - t_squar[2]
    t3t4 = t_squar[3] - t_squar[2]
    for i in range(0, 4):
        t1m = m_squar[i] - t_squar[0]
        t3m = m_squar[i] - t_squar[2]
        if np.dot(t1m, t1t2) >= 0 and np.dot(t1m, t1t4) >= 0 and np.dot(t3m, t3t2) >= 0 and np.dot(t3m, t3t4) >= 0:
            return False

    m1m2 = m_squar[1] - m_squar[0]
    m1m4 = m_squar[3] - m_squar[0]
    m3m2 = m_squar[1] - m_squar[2]
    m3m4 = m_squar[3] - m_squar[2]
    for i in range(0, 4):
        m1t = t_squar[i] - m_squar[0]
        m3t = t_squar[i] - m_squar[2]
        if np.dot(m1t, m1m2) >= 0 and np.dot(m1t, m1m4) >= 0 and np.dot(m3t, m3m2) >= 0 and np.dot(m3t, m3m4) >= 0:
            return False
    return True  # 矩形未重叠


# 判断条件
L_M = L_Fd = L_Ld = L_Fo = L_Lo = 4.5  # 所有车辆的长度
W_M = W_Fd = W_Ld = W_Fo = W_Lo = 1.8  # 所有车辆的宽度
l_M = 2.5  # 轴距
parameter_1 = parameter_2 = parameter_3 = np.array(
    ['t0', 'tc', 'x0', 'xc', 'a0', 'a1', 'a2', 'a3', 'a4', 'a5', 'b0', 'b1', 'b2', 'b3', 'b4', 'b5'])

for tc in np.arange(0.1, 12.1, 0.1):
    for xc in np.arange(1, 121):  # 12.1， 121
        r_1 = r_2 = r_3 = True
        print(tc, xc)  # 展示程序运行的进度
        for t in np.arange(0, tc, 0.1):
            '''避碰约束:'''
            if X_M1(t, tc, xc) == 0:
                yaw_angle_M = np.pi / 2
            else:
                yaw_angle_M = atan(Y_M1(t, tc) / X_M1(t, tc, xc))  # 计算主车M的每时刻的偏转角：车辆方向与纵向的的夹角

            if not (s(X_M(t, tc, xc), Y_M(t, tc), yaw_angle_M, L_M, W_M, X_Fd(t), Y_Fd(t), 0, L_Fd, W_Fd) and
                    s(X_M(t, tc, xc), Y_M(t, tc), yaw_angle_M, L_M, W_M, X_Lo(t), Y_Lo(t), 0, L_Lo, W_Lo) and
                    s(X_M(t, tc, xc), Y_M(t, tc), yaw_angle_M, L_M, W_M, X_Fo(t), Y_Fo(t), 0, L_Fo, W_Fo) and
                    s(X_M(t, tc, xc), Y_M(t, tc), yaw_angle_M, L_M, W_M, X_Ld(t), Y_Ld(t), 0, L_Ld, W_Ld) and
                    X_Ld(t) > X_M(t, tc, xc) > X_Fd(t)):  # 该轨迹该时间点碰撞 & 主车M位于Ld和Fd之间
                r_1 = r_2 = r_3 = False
                break

        '''运动学动力学约束:'''
        if r_1:
            for t in np.arange(0, tc, 0.1):
                M_curvature = abs((X_M1(t, tc, xc) * Y_M2(t, tc) - Y_M1(t, tc) * X_M2(t, tc, xc)) / (
                        X_M1(t, tc, xc) ** 2 + Y_M1(t, tc) ** 2) ** 1.5)  # 主车M的轨迹任意时刻曲率
                '''
                运动学约束（2）的计算数值,没有按照师兄的公式,!!!!!!!!!!更重要的是：k、m_delta的取值可正可负，仅仅代表左转和右转两个方向！！！！！！！！！
                正确的说，师兄计算的m_delta是错误的，如果使用我的这种计算方法，实际上k和m_delta是正线性相关的，所以运动学约束（1）、（2）可以和并成一个
                '''
                steer_angle = atan(M_curvature * l_M)  # 计算的是前轮转向角,运动学约束（1）的计算数值   !!!可能会出现0和无穷大导致的错误
                if not (steer_angle <= np.pi * 35 / 180 and 0 <= M_curvature <= 2.5 and
                        0 <= X_M1(t, tc, xc) <= 50 / 3.6 and 0 <= Y_M(t, tc) <= W):
                    r_2 = r_3 = False

        '''舒适度约束:'''
        if r_2:
            for t in np.arange(0, tc, 0.1):
                if not (abs(X_M2(t, tc, xc)) <= 1.6 and abs(Y_M2(t, tc)) <= 0.8 and abs(X_M3(t, tc, xc)) <= 1.8
                        and abs(Y_M3(t, tc)) <= 0.9):
                    r_3 = False
                    break
        # 输出轨迹到excel
        parameter_i = np.array(
            [0, tc, 0, xc, X_M(0, tc, xc), X_M1(0, tc, xc), X_M2(0, tc, xc) / 2, X_M3(0, tc, xc) / 6,
             X_M4(0, tc, xc) / 24, X_M5(0, tc, xc) / 120, Y_M(0, tc), Y_M1(0, tc), Y_M2(0, tc) / 2, Y_M3(0, tc) / 6,
             Y_M4(0, tc) / 24, Y_M5(0, tc) / 120])
        parameter_1 = np.vstack((parameter_1, parameter_i)) if r_1 else parameter_1
        parameter_2 = np.vstack((parameter_2, parameter_i)) if r_2 else parameter_2
        parameter_3 = np.vstack((parameter_3, parameter_i)) if r_3 else parameter_3

writer = pd.ExcelWriter('总数据2.xlsx')
df1 = pd.DataFrame(parameter_1)
df2 = pd.DataFrame(parameter_2)
df3 = pd.DataFrame(parameter_3)
df1.to_excel(writer, '避碰', index=0, header=0)
df2.to_excel(writer, '运动学约束', index=0, header=0)
df3.to_excel(writer, '舒适度约束', index=0, header=0)
writer.save()
