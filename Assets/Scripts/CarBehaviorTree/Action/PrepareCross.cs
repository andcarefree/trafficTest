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
    public SharedVector3[] crossLine;

    SharedVector3[] Vectors2Share(Vector3[] vector3s)
    {
        SharedVector3[] ret = new SharedVector3[vector3s.Length];
        for(int i = 0; i < ret.Length; i++)
        {
            ret[i] = vector3s[i];
        }
        return ret;
    }
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        Road roadOut = car.cross.FindRoadOut(car);
        car.cross.carRoadOut[car] = roadOut;
        lineIn = car.cross.FindLineIn(car, roadOut);
        lineOut = car.cross.FindLineOut(roadOut);
        crossLine = Vectors2Share(Line.linkLine(lineIn, lineOut));
        targetLineIndex = lineIn.indexInRoad();
        LineOutIndex = lineOut.indexInRoad();
    }
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
