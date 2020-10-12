using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class JudgeConflict : Conditional
{
    Car car;
    bool conflict = false;
    // Use this for initialization

    public override void OnTriggerEnter(Collider other)
    {
        Car otherCar = other.gameObject.GetComponent<Car>();

        if (otherCar == null) return;

        if (otherCar.line != this.car.line)//碰撞体是车辆且不是本车流车辆
        {
            Debug.LogError("发生冲突" + " " + other.name);
            conflict = true;
        }else if(otherCar.line != this.car.line)//碰撞体是同车流车辆
        {
            Debug.LogWarning("跟驰过近" + " " + car.name + " " + this.car.name);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Car>() != null)
        {
            Debug.LogWarning("冲突结束");
            conflict = false;
        }  
    }
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        if(conflict == true)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
