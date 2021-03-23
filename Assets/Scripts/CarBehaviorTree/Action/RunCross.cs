using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
/// <summary>
/// 车辆在路口内行驶的行为
/// </summary>
public class RunCross : Action
{
    Car car;
    /// <summary>
    /// 接受prepareCross中传递来的参数
    /// </summary>
    public SharedInt LineOutIndex;
    /// <summary>
    /// 初始化车辆在路口内行驶所需要的信息
    /// </summary>
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        car.state = Car.State.crossing;
        car.linePoints = car.crossLine;
    }
    /// <summary>
    /// 车辆按计算好的路径行驶
    /// </summary>
    /// <returns></returns>
    public override TaskStatus OnUpdate()
    {
        car.driving();
        //车辆的lineT超过1，说明路口中的路径已经行驶完，需要更迭车辆状态，重置行驶信息
        if(car.lineT >= 1)
        {
            car.crossLine = null;
            car.lineT = 0;
            car.setLine(car.cross.carRoadOut[car].lines[LineOutIndex.Value]);
            car.state = Car.State.inLine;
            car.cross.cars.Remove(car);
            car.preCross = car.cross;
            car.cross = null;
            car.driving();
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
