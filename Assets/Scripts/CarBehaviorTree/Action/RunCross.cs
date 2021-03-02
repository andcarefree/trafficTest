using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class RunCross : Action
{
    Car car;
    public SharedInt LineOutIndex;
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        car.state = Car.State.crossing;
        car.linePoints = car.crossLine;
    }
    public override TaskStatus OnUpdate()
    {
        car.driving();
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
