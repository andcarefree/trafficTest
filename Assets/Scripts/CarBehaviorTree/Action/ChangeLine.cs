using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 车辆的换道行为
/// </summary>
public class ChangeLine : Action
{
    Car car;
    /// <summary>
    /// 换道的目标车道
    /// </summary>
    Line targetLine;
    /// <summary>
    /// 目标车道在当前道路的下标
    /// </summary>
    public SharedInt targetLineIndex;
    /// <summary>
    /// 换道路径的生成算法，提供二次开发接口
    /// </summary>
    public static OriginCustom.CP cp=CalculatePath;
    /// <summary>
    /// 软件自身提供的简易换道路径生成算法
    /// </summary>
    public static Vector3[] CalculatePath(OCar car,OLine targetLine)
    {
        Vector3[] ret =new Vector3[4];
        ret[0] = car.transform.position;
        ret[1] = ret[0] + car.transform.forward.normalized * 3f;
        ret[2] = ret[1] + targetLine.transform.position - car.line.transform.position;
        ret[3] = ret[2] + ret[1] - ret[0];
        return ret;
    }
    /// <summary>
    /// 硬性换道路径的计算方法，用于纯模拟环境
    /// </summary>
    /// <returns></returns>
     public Vector3[] Move()
    {
        Vector3[] ret = new Vector3[2];
        ret[0] = car.transform.position;
        ret[1] = ret[0] + targetLine.transform.position - car.line.transform.position;
        return ret;
    }
    /// <summary>
    /// 车辆换道信息初始化
    /// </summary>
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        targetLine = car.line.fatherRoad.lines[targetLineIndex.Value];
        car.linePoints = cp(car,targetLine);
        //car.linePoints = Move();
        car.line.cars.Remove(car);
        //行驶路径初始化
        car.line = null;
        car.lineT = 0;
        car.target = car.linePoints[0];
    }
    /// <summary>
    /// 车辆在换道信息初始化之后开始进入换道过程
    /// </summary>
    /// <returns></returns>
    public override TaskStatus OnUpdate()
    {
        car.accel = 5;
        car.driving();
        if(car.lineT >= 1)
        {
            End();
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    /// <summary>
    /// 换道结束时调用，重置车辆的形式信息
    /// </summary>
    private void End()
    {
        car.changeLine(targetLine);
        car.lineT = Line.CalculateT(car.transform.position, car.line.points);
        car.state = Car.State.inLine;
        car.lineChange = false;
        car.driving();
    }
}
