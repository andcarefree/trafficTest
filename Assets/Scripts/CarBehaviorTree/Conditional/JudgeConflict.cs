using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
public class JudgeConflict : Conditional
{
    Car car;
    /// <summary>
    /// 标志是否冲突的标志位
    /// </summary>
    bool conflict = false;
    /// <summary>
    /// 车辆的碰撞体触发，说明发生了冲突
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Car>() != null)
        {
            Car otherCar = other.gameObject.GetComponent<Car>();
            //在道路中行驶的车辆不会碰撞
            if (car.state == OCar.State.inLine && otherCar.state == OCar.State.inLine)
            {
                return;
            }
            //发生冲突时，选择一辆车继续行驶，一辆让行
            Car lucky = CollisionSystem.ChooseLucky(car, otherCar);
            var value = CollisionSystem.LineLineIntersection(car.transform.position, car.transform.forward, otherCar.transform.position, otherCar.transform.forward);
            //尾部碰撞器，两车不会碰撞
            if (value == Vector3.zero)
            {
                return;
            }
            //为让行车辆构造碰撞体
            var newBarrier = new Barrier();
            newBarrier.position = value;
            //设置otherCar的barrier
            if (lucky == car)
            {
                if(otherCar.barrier == null || Vector3.Distance(otherCar.barrier.position,otherCar.transform.position) >= Vector3.Distance(newBarrier.position, otherCar.transform.position)){
                    otherCar.barrier = newBarrier;
                }
            }
            //设置Car的barrier
            if (lucky == otherCar)
            {
                if(car.barrier == null || Vector3.Distance(car.barrier.position, car.transform.position) >= Vector3.Distance(newBarrier.position, car.transform.position))
                {
                    car.barrier = newBarrier;
                }
            }
        }
    }
    public override void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Car>() != null && conflict == true)
        {
            //Debug.LogWarning("冲突结束");
            conflict = false;
        }
    }
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }
    /// <summary>
    /// 发生冲突即将车辆碰撞标志位置为真
    /// </summary>
    /// <returns></returns>
    public override TaskStatus OnUpdate()
    {
        if (car.barrier != null)
        {
            conflict = true;
        }
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
