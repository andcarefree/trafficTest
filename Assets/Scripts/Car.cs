using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// TODO 车辆的速度与加速度处理需要重新设计使其符合现实情境
public class Car : OCar
{
    
    const float MaxVelocityNoRoad = 30;

    /*
    public enum State
    {
        inLine,
        crossing,
        changing,
        prepareCross
    }
    */

    public Cross cross;
    public Cross preCross;
    public Vector3[] crossLine;

    /*
    public State state = State.inLine;
    /// <summary>
    /// 加速度,单位m/s
    /// </summary>
    public float accel = 0;
    /// <summary>
    /// 最大加速度
    /// </summary>
    public float maxAccel;
    /// <summary>
    /// 速度,单位km/h
    /// </summary>
    public float velocity = 0;
    

    /// <summary>
    /// 路径长度
    /// </summary>
    public float s = 0;

    */


    /// <summary>
    /// 所在路线
    /// </summary>

    public new Line line
    {
        get { return (Line)base.line; }
        set { base.line = value; }
    }
    /*
    /// <summary>
    /// 路线的T参数
    /// </summary>
    public float lineT;
    /// <summary>
    /// 路点信息
    /// </summary>
    /// 
    public Vector3[] linePoints;
    public float segment;
    /// <summary>
    /// 目标点
    /// </summary>
    public Vector3 target = new Vector3(1, 0, 0);

    public float expectVelocity;
    public static float[] expects = { 30, 40, 50, 60, 70 };

    /// <summary>
    /// 修改该属性发出换道指令
    /// </summary>
    public bool lineChange = false;

    /// <summary>
    /// 车辆突然停下的测试
    /// </summary>
    public bool stopTest = false;
    
    
    /// <summary>
    /// 车辆跟驰对象，通过该对象限制加速度（包括路内跟驰，换道，路口内所有情形）
    /// 解决冲突问题，车辆与阻碍其行驶的车辆构成一个跟驰行为，确保不会相撞
    /// 维护一个临近范围车辆集，当触发车辆进入时即加入该集合，集合内寻找一个disOfForward最短的做跟驰
    /// </summary>
    public Car followCar;
    */

    void Awake()
    {
        this.expectVelocity = Car.expects[(int)Random.Range(0f, (float)Car.expects.Length)];
    }

    void Start()
    {
        GameEvents.current.OnLoadEvent += DestroyCar;
        if (RectangleSelector.current != null)
            RectangleSelector.current.Selectable.Add(this.gameObject);
    }

    void OnDestroy()
    {
        if (RectangleSelector.current != null)
        {
            RectangleSelector.current.Selectable.Remove(this.gameObject);
            RectangleSelector.current.Selected.Remove(this.gameObject);
        }
    }
    /*
    /// <summary>
    /// 将车辆速度转换为m/s
    /// </summary>
    public new float Km2m()
    {
        return this.velocity / 3.6f;
    }
    */
    /*
    public new Car PreCar()
    {
        return (Car)base.PreCar();
    }
    */
    public new Car PreCar()
    {
        if (this.line.cars.Find(this) == null)
        {
            return null;
        }
        if (this.line.cars.Find(this).Previous == null)
        {
            return null;
        }
        return (Car)this.line.cars.Find(this).Previous.Value;
    }
    
    /*
    public new Car NextCar()
    {
        return (Car)base.NextCar();
    }*/
    
    
    public new Car NextCar()
    {
        if (this.line.cars.Find(this) == null)
        {
            return null;
        }
        if (this.line.cars.Find(this).Next == null)
        {
            return null;
        }
        return (Car)this.line.cars.Find(this).Next.Value;
    }
    
    public Car CarClosest(Line line)
    {
        if (line.cars.First == null)
        {
            return null;
        }

        Car pointer = (Car)line.cars.First.Value;
        while (pointer.NextCar() != null)
        {
            if (Vector3.Distance(this.transform.position, pointer.transform.position) < Vector3.Distance(this.transform.position, pointer.NextCar().transform.position))
            {
                break;
            }
            else
            {
                pointer = pointer.NextCar();
            }
        }
        return pointer;
    }

    public void DestroyCar()
    {
        if (this.line != null)
        {
            this.line.cars.Remove(this);
        }
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

    /// <summary>
    /// target在pointer之后则返回真
    /// </summary>
    public static bool judgeLocation(Car pointer, Car target)
    {
        if (pointer == null || target == null)
        {
            return true;
        }
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

    public Car findNextCar(Line l)
    {
        LinkedListNode<OCar> pointer = l.cars.First;
        while (pointer != null)
        {
            //在该车流中找到第一个在car后面的车辆
            if (!judgeLocation(pointer.Value, this))
            {
                return (Car)pointer.Value;
            }
            pointer = pointer.Next;
        }
        return null;
    }

    public void changeLine(Line l)
    {
        Car target = findNextCar(l);
        if (target == null)
        {
            //车流未找到插入位置，在末端插入
            l.cars.AddLast(this);
            this.line = l;
            this.linePoints = l.points;
        }
        else
        {
            l.cars.AddBefore(l.cars.Find(target), this);
            this.line = l;
            this.linePoints = l.points;
        }
    }

    /// <summary>
    /// 找到路口内路径，以点组形式返回
    /// 会改变车辆的line与linePoints
    /// </summary>
    public void findPath()
    {
        if (state == State.inLine) return;
        int rdm1 = Random.Range(0, line.nextRoads.Length - 1);//确定道路
        int rdm2 = 0;
        //找到车辆数最少的车道
        for (int i = 0; i < line.nextRoads[rdm1].lines.Length; i++)
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
        while (Vector3.Distance(target, transform.position) <= 2f)
        {
            lineT += (float)1 / (float)segment;
            target = Line.Bezier(lineT, linePoints);
        }

        //车辆朝向目标点
        if (target != transform.position)
            transform.LookAt(target);

        if (stopTest == true)
        {
            velocity = 0;
            return;
        }

        //道路限速；车辆期望速度；正常行驶速度；取最小值
        velocity = Mathf.Min(this.line == null ? Car.MaxVelocityNoRoad : this.line.maxVelocity, velocity + 3.6f * accel * Time.deltaTime, expectVelocity);
        //屏蔽掉速度小于0的倒车行为
        velocity = Mathf.Max(0, velocity);
        s += Km2m() * Time.deltaTime;
        this.transform.Translate(Vector3.forward * Km2m() * Time.deltaTime);
    }

    public float disOfForward(Car other)
    {
        Vector3 forward = this.transform.forward.normalized;
        Vector3 spacing = other.transform.position - this.transform.position;
        return Vector3.Dot(forward, spacing);
    }


}
