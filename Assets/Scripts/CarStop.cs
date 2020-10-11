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
        car.accel = -30;
        if(car.velocity <= 5)
        {
            car.velocity = 0;
        }

        car.driving();

        if (car.lineT >= 1)
        {
            car.lineT = 0;
            //车辆行驶完所在道路并且没有后续道路
            if (car.state == Car.State.inLine && (car.line.nextRoads == null || car.line.nextRoads.Length == 0))
            {
                car.DestroyCar();
                return TaskStatus.Success;
            }
            //道路与路口的转换
            else if (car.state == Car.State.inLine)
            {
                --car.line.carNumber;
                car.state = Car.State.crossing;
            }
            else
            {
                car.state = Car.State.inLine;
                car.setLine(car.line);
            }
        }

        return TaskStatus.Running;
    }
}
