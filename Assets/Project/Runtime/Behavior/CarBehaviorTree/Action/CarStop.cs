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

    public override TaskStatus OnUpdate()
    {
        //TODO 逐渐减速过程的细化，注意处理加速度的变化趋势
        //car.velocity = 10;
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

    public override void OnEnd()
    {
        //触发停止的时间结束时开始加速
        car.accel = 30;
    }
}
