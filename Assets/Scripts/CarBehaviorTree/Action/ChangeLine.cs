using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ChangeLine : Action
{
    Car car;
    Line targetLine;
    Vector3[] runPoints;

    /**/
    float T;
    /**/

    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        /*实验代码*/
        car.line.cars.Remove(car);
        targetLine = car.line.fatherRoad.lines[1];
        T = car.lineT;
        car.lineT = 0;
        if(car.line == car.line.fatherRoad.lines[0])
        {
            runPoints = new Vector3[] { car.transform.position, car.transform.position - new Vector3(0, 0, 10) };
        }
        else
        {
            runPoints = new Vector3[] { car.transform.position, car.transform.position + new Vector3(0, 0, 10) };
        }
        //1. 确定换道轨迹
        car.linePoints = runPoints;
        /*实验代码*/
    }

    public override TaskStatus OnUpdate()
    {
        car.velocity = 10;
        car.driving();
        if(car.lineT >= 1)
        {
            //2。换道成功后改变属性
            car.changeLine(targetLine);
            car.lineT = T;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        car.lineChange = false;
    }
}
