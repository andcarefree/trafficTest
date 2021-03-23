using BehaviorDesigner.Runtime.Tasks;
/// <summary>
/// 车辆是否位于路口中的判断
/// </summary>
public class IsInCross : Conditional
{
    Car car;
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }
    /// <summary>
    /// 判断车辆是否处于或即将处于路口之中
    /// </summary>
    /// <returns></returns>
    public override TaskStatus OnUpdate()
    {
        //车辆所处车道没有下一条道路
        if(car.line != null && car.line.nextRoads.Count == 0)
        {
            return  TaskStatus.Failure;
        }
        //车俩处于路口准备或路口中的状态位，则该车辆在路口中
        if(car.state == Car.State.prepareCross || car.state == Car.State.crossing)
        {
            return TaskStatus.Success;
        }
        //车辆已经计算好了将要在路口中行驶的路径，说明已经做好了路口行驶的准备
        if(car.state == Car.State.inLine && car.crossLine != null && car.crossLine.Length != 0)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
