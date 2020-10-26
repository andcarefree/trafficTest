using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class JudgeConflict : Conditional
{
    Car car;
    bool conflict = false;

    public override void OnTriggerEnter(Collider other)
    {
        Car otherCar = other.gameObject.GetComponent<Car>();

        if (otherCar == null) return;//碰撞体非车辆

        if(otherCar.line == this.car.line)//碰撞体是同车流车辆
        {
            Debug.LogWarning("跟驰过近" + " " + car.transform.position + " " + this.car.transform.position);
            return;
        }

        //TODO 车辆间发生冲突
        //现在的做法是如果车道中的车辆会影响到换道中的车辆，始终让车道中的车辆停车减速
        //但是多车换道时情况过于复杂，难以全面处理
        //1.可以在换道策略中限制换道行为，屏蔽一些不规范换道
        //2.修改碰撞器形状与触发方法，拓展车辆之间的通信
        if(car.state == Car.State.inLine && !Car.judgeLocation(car, otherCar))
        {
            Debug.LogError("道路行车需要让行" + " " + car.transform.position);
            conflict = true;
            return;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Car>() != null && conflict == true)
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
