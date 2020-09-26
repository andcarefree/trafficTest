using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CarStop : Action
{
    Car car;
    // Use this for initialization
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        car.accel = 0;
        car.velocity = 0;
        return TaskStatus.Running;
    }
}
