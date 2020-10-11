using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ChangeLine : Action
{
    Car car;

    public void changeLine(Line now ,Line target)
    {

    }
    // Use this for initialization
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
