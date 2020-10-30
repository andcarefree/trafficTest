using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 任何情况下车辆的减速停车行为
/// </summary>
public class CarStop : Action
{
    Car car;

    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

    public override TaskStatus OnUpdate()
    {
        //TODO 逐渐减速过程的细化，注意处理加速度的变化趋势
        car.velocity = 10;
        car.driving();

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        //触发停止的时间结束时开始加速
        car.accel = 30;
    }
}
