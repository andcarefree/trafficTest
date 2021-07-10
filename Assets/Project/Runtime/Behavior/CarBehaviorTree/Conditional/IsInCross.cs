using BehaviorDesigner.Runtime.Tasks;

public class IsInCross : Conditional
{
    Car car;
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

    public override TaskStatus OnUpdate()
    {
        if(car.line != null && car.line.nextRoads.Count == 0)
        {
            return  TaskStatus.Failure;
        }
        if(car.state == Car.State.prepareCross || car.state == Car.State.crossing)
        {
            return TaskStatus.Success;
        }
        if(car.state == Car.State.inLine && car.crossLine != null && car.crossLine.Length != 0)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
