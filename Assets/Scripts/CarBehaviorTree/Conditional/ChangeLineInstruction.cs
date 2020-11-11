using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

//通过改变car.lineChange发出对车辆的换道指令
public class ChangeLineInstruction : Conditional
{
    Car car;

    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

    public override TaskStatus OnUpdate()
    {
        //即将驶入路口，不予换道
        // TODO 考虑到曲线道路，判断车辆点与终点的直线长度可能会有问题
        //可以改换到T参数判断？
        if (car.lineChange == true && car.state == Car.State.inLine && Vector3.Distance(car.transform.position, car.line.points[car.line.points.Length - 1]) <= 14)
        {
            //重置换道指令
            if (car.lineChange == true)
            {
                car.lineChange = false;
            }
            return TaskStatus.Failure;
        }

        //换道条件判断
        //当前车道运行速度低于预期时，寻求车道换道
        if (car.line != null && car.line.cars.First != null && car.line.cars.First.Value != car && car.s>10)
        {
            if (Vector3.Distance(car.line.cars.Find(car).Previous.Value.transform.position, car.transform.position) > 3)
            {

            }
            else if (car.line.cars.Find(car).Previous.Value.velocity <= 40 && car.expectVelocity - car.velocity > 5)
            {
                car.lineChange = true;
            }
            else if (car.line.cars.Find(car).Previous.Value.velocity >= 40 && car.expectVelocity - car.velocity > 15)
            {
                car.lineChange = true;
            }
        }

        if (car.state == Car.State.changing || (car.lineChange == true && car.state == Car.State.inLine))
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }

    }
}
