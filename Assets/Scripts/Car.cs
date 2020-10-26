using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// TODO 车辆的速度与加速度处理需要重新设计使其符合现实情境
public class Car:MonoBehaviour
{
    public enum State
    {
        inLine,
        crossing,
        changing
    }

    public State state = State.inLine;
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

    /**/
    public bool lineChange = false;
    /**/
    public void DestroyCar()
    {
        this.line.cars.Remove(this);
        Destroy(this.gameObject);
    }

    public void setLine(Line l)
    {
        if (this.line != null)
        {
            this.line.cars.Remove(this);
        }
        line = l;
        lineT = 0;
        linePoints = l.points;
        segment = Line.segmentNum;
        l.cars.AddLast(this);
    }

    public static bool judgeLocation(Car pointer, Car target)
    {
        Vector3 dir1 = pointer.transform.forward.normalized;
        Vector3 dir2 = (target.transform.position - pointer.transform.position).normalized;
        if (Vector3.Dot(dir1, dir2) < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void changeLine(Line l)
    {
        LinkedListNode<Car> pointer = l.cars.First;
        while (pointer != null)
        {
            //判断车辆插入位置,要考虑到车辆坐标与朝向
            //在该车流中找到第一个在car后面的车辆，并在其之前插入
            if (!judgeLocation(pointer.Value, this))
            {
                l.cars.AddBefore(pointer, this);
                this.line = l;
                this.linePoints = l.points;
                return;
            }
            pointer = pointer.Next;
        }
        //车流未找到插入位置，在末端插入
        l.cars.AddLast(this);
        this.line = l;
        this.linePoints = l.points;
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
            rdm2 = line.nextRoads[rdm1].lines[i].cars.Count < line.nextRoads[rdm1].lines[rdm2].cars.Count ? i : rdm2;
        }
        //int rdm2 = Random.Range(0, line.nextRoads[rdm1].lines.Length-1);//确定车道
        Line nextLine = line.nextRoads[rdm1].lines[rdm2];
        //linePoints = Line.Interpolation(line, nextLine);
        linePoints = Line.linkLine(line, nextLine);
        line = nextLine;
    }

    public void driving()
    {
        //更新linT与下一个目标点
        //如果车辆目标点与车辆所在位置差距过大，则按算法更新目标点
        while (Vector3.Distance(target,transform.position) <= 2f)
        {
            lineT += (float)1 / (float)segment;
            target = Line.Bezier(lineT, linePoints);
        }

        //车辆朝向目标点
        if (target != transform.position)
            transform.LookAt(target);

        //更新车辆速度与位移
        velocity = Mathf.Min(maxVelocity, velocity + accel * Time.deltaTime);
        //屏蔽掉速度小于0的倒车行为
        velocity = Mathf.Max(0, velocity);
        s += velocity * Time.deltaTime / 3.6f;
        this.transform.Translate(Vector3.forward * velocity * Time.deltaTime / 3.6f);
    }

    void OnCollisionEnter(Collision collision)
    {
        //这个函数在碰撞开始时候调用
        Debug.LogError("发生碰撞");
    }
}
