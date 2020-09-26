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
        //更新linT与下一个目标点
        //如果车辆目标点与车辆所在位置差距过大，则按算法更新目标点
        while (Vector3.Distance(car.target, car.transform.position) <= 2f)
        {
            car.lineT += (float)1 / (float)car.segment;
            car.target = Line.Bezier(car.lineT, car.linePoints);
        }

        //更新车辆加速度
        if (car.front == null)
        {
            car.accel = Random.Range(1, 11);
        }
        else if (car.front.s - car.s >= 120)
        {
            car.accel = car.front.velocity - car.velocity + 10;
        }
        else
        {
            car.accel = 200 * (car.front.velocity - car.velocity) / (car.front.s - car.s);
        }
       
        //车辆朝向目标点
        car.carTurn();

        //更新车辆速度与位移
        car.velocity = Mathf.Min(car.maxVelocity, car.velocity + car.accel * Time.deltaTime);
        car.s += car.velocity * Time.deltaTime / 3.6f;
        this.transform.Translate(Vector3.forward * car.velocity * Time.deltaTime / 3.6f);

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
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

}
