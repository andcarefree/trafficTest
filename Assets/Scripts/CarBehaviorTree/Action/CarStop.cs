using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 任何情况下车辆的减速停车行为
/// </summary>
public class CarStop : Action
{
    Car car;
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }
    /// <summary>
    /// 车辆的让行行为
    /// </summary>
    /// <returns></returns>
    public override TaskStatus OnUpdate()
    {
        if(Vector3.Distance(car.barrier.position,car.transform.position)-car.barrier.radius <= 0.5f+car.transform.localScale.z)
        {
            car.accel = 0;
            car.velocity = 0;
        }
        else
        {
            car.accel = Mathf.Pow(car.Km2m(), 1.5f) * (0 - car.Km2m()) / Mathf.Pow(car.barrier.s - car.barrier.radius - car.s, 0.9f);
        }
        car.driving();
        return TaskStatus.Running;
    }
    /// <summary>
    /// 冲突结束时车辆开始尝试加速行驶
    /// </summary>
    public override void OnEnd()
    {
        //停止的时间结束时开始加速
        car.accel = 30;
    }
}
