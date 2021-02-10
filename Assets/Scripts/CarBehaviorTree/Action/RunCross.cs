using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class RunCross : Action
{
    Car car;
    public SharedVector3[] crossLine;
    public SharedInt LineOutIndex;

    Vector3[] Share2Vectors(SharedVector3[] sharedVector3s)
    {
        Vector3[] ret = new Vector3[sharedVector3s.Length];
        for(int i = 0; i < sharedVector3s.Length; i++)
        {
            ret[i] = sharedVector3s[i].Value;
        }
        return ret;
    }
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        car.state = Car.State.crossing;
        car.linePoints = Share2Vectors(crossLine);
    }
    public override TaskStatus OnUpdate()
    {
        car.driving();
        if(car.lineT >= 1)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnd()
    {
        car.lineT = 0;
        car.line = car.cross.carRoadOut[car].lines[LineOutIndex.Value];
        car.state = Car.State.inLine;
        car.cross.cars.Remove(car);
        car.cross = null;
        car.driving();
    }
}
