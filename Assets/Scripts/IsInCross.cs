using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsInCross : Conditional
{
    Car car;
    // Start is called before the first frame update
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        if (judgeCarInCross())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
    private bool judgeCarInCross()
    {
        if (car.state == Car.State.crossing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
