using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Following : Action
{
    Car car;
    public static OriginCustom.GM gm=OriginGM;
    //public delegate float GM(float c, float m, float l, OriginCar previous);
    

    /// <summary>
    /// GM跟驰模型
    /// </summary>
    /// <param name="c">车辆灵敏度</param>
    private static float OriginGM(OriginCar m_car,float c,float m,float l,OriginCar previous)
    {
        return c * Mathf.Pow(m_car.Km2m(), m) * (previous.Km2m() - m_car.Km2m()) / Mathf.Pow(previous.s - m_car.s, l);
    }

    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        car.target = car.transform.position;
        
    }

    
    public override TaskStatus OnUpdate()
    {
        //TODO
        if(car.state != Car.State.inLine)
        {
            car.accel = 0;
            car.velocity = 30;
        }
        else
        {
            if (car.line.cars.Find(car).Previous == null)
            {
                if(car.velocity <= car.expectVelocity)
                {
                    car.accel = Random.Range(1, 11);
                }
                else
                {
                    car.accel = Random.Range(-5, 5);
                }  
            }
            else
            {
                Car previous = car.line.cars.Find(car).Previous.Value;
                //车头时距小于等于5s，车辆进入跟驰状态
                if ((previous.s - car.s - car.transform.localScale.z) / car.Km2m() <= 5)
                {
                    /*if (DllReader.gm == null)
                        car.accel = OriginGM(car,1, 1.5f, 0.9f, previous);
                    else
                        car.accel=(float)DllReader.gm.Invoke(DllReader.custom,new object[] {car, 1, 1.5f, 0.9f, previous });*/
                    car.accel = gm(car, 1, 1.5f, 0.9f, previous);

                }
                else
                {
                    car.accel = 5;
                }
            }
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
            if (car.state == Car.State.inLine)
            {
                car.line.cars.Remove(car.line.cars.Find(car));
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Running;
    }

}
