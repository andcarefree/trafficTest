using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class PrepareCross : Action
{
    Car car;
    Line lineIn;
    Line lineOut;

    public SharedInt targetLineIndex;
    public SharedInt LineOutIndex;
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }
    public override TaskStatus OnUpdate()
    {
        Road roadOut = car.cross.FindRoadOut(car);
        car.cross.carRoadOut[car] = roadOut;
        lineIn = car.cross.FindLineIn(car, roadOut);
        lineOut = car.cross.FindLineOut(roadOut);
        car.crossLine = Line.linkLine(lineIn, lineOut);
        targetLineIndex.Value = lineIn.indexInRoad();
        LineOutIndex.Value = lineOut.indexInRoad();
        return TaskStatus.Success;
    }
}
