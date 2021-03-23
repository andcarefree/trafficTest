using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
/// <summary>
/// 车辆在同一车道上的跟驰行驶行为
/// </summary>
public class Following : Action
{
    Car car;
    /// <summary>
    /// 车辆跟驰算法的二次开发接口
    /// </summary>
    public static OriginCustom.GM gm=OriginGM;
    //public delegate float GM(float c, float m, float l, OCar previous);
    /// <summary>
    /// GM跟驰模型
    /// </summary>
    /// <param name="c">车辆灵敏度</param>
    private static float OriginGM(OCar m_car,float c,float m,float l,OCar previous)
    {
        return c * Mathf.Pow(m_car.Km2m(), m) * (previous.Km2m() - m_car.Km2m()) / Mathf.Pow(previous.s - m_car.s, l);
    }
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        car.target = car.transform.position;
    }
    /// <summary>
    /// 车辆在车道中行驶时默认处于跟驰行为中，确保不会碰撞前车
    /// </summary>
    /// <returns></returns>
    public override TaskStatus OnUpdate()
    {
        if (car.state != Car.State.inLine)
        {
            car.accel = 0;
            car.velocity = 30;
        }
        else
        {
            if (car.line.cars.Find(car).Previous == null)
            {
                if (car.velocity <= car.expectVelocity)
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
                Car previous = (Car)car.line.cars.Find(car).Previous.Value;
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
        //车辆的lineT参数大于等于一，说明已经行驶完了预定的行驶路径，需要进行状态的更迭
        if (car.lineT >= 1)
        {
            car.lineT = 0;
            //车辆行驶完所在道路并且没有后续道路，则车辆已经到达目标终点，停止行驶
            if (car.state == Car.State.inLine && (car.line.nextRoads == null || car.line.nextRoads.Count == 0))
            {
                car.DestroyCar();
                return TaskStatus.Success;
            }
            //车辆进入路口行驶
            if (car.state == Car.State.inLine)
            {
                car.line.cars.Remove(car.line.cars.Find(car));
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Running;
    }
}
