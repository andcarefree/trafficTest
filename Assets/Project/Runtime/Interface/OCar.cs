using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;



public class OCar : MonoBehaviour
{
    const float MaxVelocityNoRoad = 30;
    public enum State
    {
        inLine,
        crossing,
        changing,
        prepareCross
    }

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
    /// <summary>
    /// 所在路线
    /// </summary>
    public OLine line;
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
    public OCar followCar;

    public float Km2m()
    {
        return this.velocity / 3.6f;
    }

    public OCar PreCar()
    {
        if (this.line.cars.Find(this) == null)
        {
            return null;
        }
        if (this.line.cars.Find(this).Previous == null)
        {
            return null;
        }
        return this.line.cars.Find(this).Previous.Value;
    }
    public OCar NextCar()
    {
        if (this.line.cars.Find(this) == null)
        {
            return null;
        }
        if (this.line.cars.Find(this).Next == null)
        {
            return null;
        }
        return this.line.cars.Find(this).Next.Value;
    }
    public OCar CarClosest(OLine line)
    {
        if (line.cars.First == null)
        {
            return null;
        }

        OCar pointer = line.cars.First.Value;
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
    public static bool judgeLocation(OCar pointer, OCar target)
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
}

