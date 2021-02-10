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
        if(car.state == Car.State.prepareCross || car.state == Car.State.crossing)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
