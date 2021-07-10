using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class JudgeConflict : Conditional
{
    Car car;
    bool conflict = false;

    public override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Car>() != null)
        {
            Car otherCar = other.gameObject.GetComponent<Car>();
            if (car.state == OCar.State.inLine && otherCar.state == OCar.State.inLine)//道路中行驶的车辆不会碰撞
            {
                return;
            }
            Car lucky = CollisionSystem.ChooseLucky(car, otherCar);
            var value = CollisionSystem.LineLineIntersection(car.transform.position, car.transform.forward, otherCar.transform.position, otherCar.transform.forward);
            if(value == Vector3.zero)//尾部碰撞器，两车不会碰撞
            {
                return;
            }
            var newBarrier = new Barrier();
            newBarrier.position = value;
            if(lucky == car)//设置otherCar的barrier
            {
                if(otherCar.barrier == null || Vector3.Distance(otherCar.barrier.position,otherCar.transform.position) >= Vector3.Distance(newBarrier.position, otherCar.transform.position)){
                    otherCar.barrier = newBarrier;
                }
            }
            if(lucky == otherCar)//设置Car的barrier
            {
                if(car.barrier == null || Vector3.Distance(car.barrier.position, car.transform.position) >= Vector3.Distance(newBarrier.position, car.transform.position))
                {
                    car.barrier = newBarrier;
                }
            }
        }
    }

    /*public override void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Car>() != null && conflict == true)
        {
            //Debug.LogWarning("冲突结束");
            conflict = false;
        }
    }*/

    //TODO 车辆间发生冲突
            //现在的做法是如果车道中的车辆会影响到换道中的车辆，始终让车道中的车辆停车减速
            //但是多车换道时情况过于复杂，难以全面处理
            //1.可以在换道策略中限制换道行为，屏蔽一些不规范换道
            //2.修改碰撞器形状与触发方法，拓展车辆之间的通信
            /*if (car.state == Car.State.inLine && otherCar.state == Car.State.changing && !Car.judgeLocation(car, otherCar))
            {
                //Debug.LogWarning("道路行车需要让行" + " " + car.transform.position);
                conflict = true;
                return;
            }*/
public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

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
