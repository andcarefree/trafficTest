using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class JudgeLineLock : Conditional
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
        //车辆判断锁持有
        //锁无人持有或锁自己持有，则加锁并继续运行
        if(car.lineT >= 0.8 && (car.line.lineLock == null||car.line.lineLock == car))
        {
            car.line.lineLock = car;
            return TaskStatus.Success;
        }
        //锁非本车辆持有，放弃抢占
        else if (car.lineT >= 0.8 && car.line.lineLock != car)
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}
