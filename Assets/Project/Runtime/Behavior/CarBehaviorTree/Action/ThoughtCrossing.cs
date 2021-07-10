using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ThoughtCrossing : Action
{
    Car car;
    Line runLine;

    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }
    //当前的问题在于，当多次调用该脚本时，车辆line的判断问题
    public override TaskStatus OnUpdate()
    {
        car.findPath();
        return TaskStatus.Success;
    }
}
