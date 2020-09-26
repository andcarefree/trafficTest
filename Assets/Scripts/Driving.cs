using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class Driving : Action
{
    Car car;
    // Use this for initialization
    public override void OnAwake()
    {
        car = gameObject.GetComponent<Car>();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        
        return TaskStatus.Success;
    }
}
