using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class JudgeLight : Conditional
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
        if (judgeLight())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
        
    }
    private bool judgeLight()
    {
        if (car.line.trafficState == TrafficStateEnum.PASS)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
