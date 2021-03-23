using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
/// <summary>
/// 车辆进入路口行驶之前的准备行为，需要计算并初始化一些数据
/// </summary>
public class PrepareCross : Action
{
    Car car;
    /// <summary>
    /// 车辆驶入路口的车道
    /// </summary>
    Line lineIn;
    /// <summary>
    /// 车辆驶出路口的车道
    /// </summary>
    Line lineOut;
    /// <summary>
    /// 车辆驶入路口车道在道路中的下标
    /// </summary>
    public SharedInt targetLineIndex;
    /// <summary>
    /// 车辆驶出路口车道在道路中的下标
    /// </summary>
    public SharedInt LineOutIndex;
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }
    /// <summary>
    /// 初始化车辆在路口中行驶需要的所有信息
    /// </summary>
    /// <returns></returns>
    public override TaskStatus OnUpdate()
    {
        //车道末尾没有接下一条路，快速失败
        if(car.line.nextRoads.Count == 0)
        {
            return TaskStatus.Failure;
        }
        //包装驶出道路
        Road roadOut = car.cross.FindRoadOut(car);
        car.cross.carRoadOut[car] = roadOut;
        //确定驶入车道
        lineIn = car.cross.FindLineIn(car, roadOut);
        //确定驶出车道
        lineOut = car.cross.FindLineOut(roadOut,lineIn);
        //由驶入车道与驶出车道计算路口内路径
        car.crossLine = Line.linkLine2(lineIn, lineOut);
        //传递进入路口钱确定的初始信息
        targetLineIndex.Value = lineIn.indexInRoad();
        LineOutIndex.Value = lineOut.indexInRoad();
        return TaskStatus.Success;
    }
}
