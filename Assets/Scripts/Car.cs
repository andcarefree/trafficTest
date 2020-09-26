using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Car:MonoBehaviour
{
    public enum State
    {
        inLine,
        crossing
    }
    public enum RunStateEnum
    {
        RUN,
        STOP
    }

    public State state = State.inLine;
    public RunStateEnum runState = RunStateEnum.RUN;
    /// <summary>
    /// 加速度
    /// </summary>
    public float accel = 0;
    /// <summary>
    /// 最大加速度
    /// </summary>
    public float maxAccel;
    /// <summary>
    /// 速度
    /// </summary>
    public float velocity = 0;
    /// <summary>
    /// 限速
    /// </summary>
    public float maxVelocity = 60;
    /// <summary>
    /// 路径长度
    /// </summary>
    public float s = 0;

    /// <summary>
    /// 所在路线
    /// </summary>
    public Line line;
    /// <summary>
    /// 路线的T参数
    /// </summary>
    public float lineT;
    /// <summary>
    /// 路点信息
    /// </summary>
    public Vector3[] linePoints;
    public float segment;
    /// <summary>
    /// 目标点
    /// </summary>
    public Vector3 target = new Vector3(1, 0, 0);

    public Car front,behind;

    public void DestroyCar()
    {
        if (behind != null)
            behind.front = null;
        --gameObject.GetComponent<Car>().line.carNumber;
        Destroy(this.gameObject);
    }

    public void setLine(Line l)
    {
        ++l.carNumber;
        if(behind != null)
        {
            behind.front = null;
            behind = null;
        }
        line = l;
        lineT = 0;
        linePoints = l.points;
        segment = Line.segmentNum;
        if (l.lastCar != null)
            l.lastCar.behind = this;
        front = l.lastCar;
        l.lastCar = this;
    }

    /// <summary>
    /// 车辆转向
    /// </summary>
    public void carTurn()
    {
        if (target != transform.position)
            transform.LookAt(target);
    }

    /// <summary>
    /// 找到路口内路径，以点组形式返回
    /// 会改变车辆的line与linePoints
    /// </summary>
    public void findPath()
    {
        if (state == State.inLine) return;
        int rdm1 = Random.Range(0, line.nextRoads.Length-1);//确定道路
        int rdm2 = 0;
        //找到车辆数最少的车道
        for(int i = 0; i < line.nextRoads[rdm1].lines.Length; i++)
        {
            rdm2 = line.nextRoads[rdm1].lines[i].carNumber < line.nextRoads[rdm1].lines[rdm2].carNumber ? i : rdm2;
        }
        //int rdm2 = Random.Range(0, line.nextRoads[rdm1].lines.Length-1);//确定车道
        Line nextLine = line.nextRoads[rdm1].lines[rdm2];
        //linePoints = Line.Interpolation(line, nextLine);
        linePoints = Line.linkLine(line, nextLine);
        line = nextLine;
    }
}
