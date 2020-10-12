using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Following : Action
{
    Car car;
    public override void OnAwake()
    {
        car = gameObject.GetComponent<Car>();
        car.target = car.transform.position;
    }

    //跟驰要额外考虑到前车停下来的话，后车在一个触发距离内也能够停车
    public override TaskStatus OnUpdate()
    {
        //更新车辆加速度
        if (car.line.cars.First.Value == car)
        {
            car.accel = Random.Range(1, 11);
        }
        else if (car.line.cars.Find(car).Previous.Value.s - car.s >= 120)
        {
            car.accel = car.line.cars.Find(car).Previous.Value.velocity - car.velocity + 10;
        }
        else
        {
            //如果车辆合流时出现重叠或者间距更近，会发生车辆急速后跳的现象
            car.accel = 200 * (car.line.cars.Find(car).Previous.Value.velocity - car.velocity) / (car.line.cars.Find(car).Previous.Value.s - car.s);
        }

        car.driving();

        //释放锁
        if(car.line.lineLock == car && car.state == Car.State.inLine&& car.lineT >= 0.05)
        {
            car.line.lineLock = null;
        }

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
