using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class ChangeLine : Action
{
    Car car;
    Line targetLine;

    public SharedInt targetLineIndex;

    /// <summary>
    /// 换道路径生成算法
    /// ///TODO 该算法应该是可供二次开发的
    /// </summary>
    public Vector3[] CalculatePath()
    {
        Vector3[] ret =new Vector3[4];
        ret[0] = car.transform.position;
        ret[1] = ret[0] + car.transform.forward.normalized * 4.6f;
        ret[2] = ret[1] + targetLine.transform.position - car.line.transform.position;
        ret[3] = ret[2] + ret[1] - ret[0];
        return ret;
    }

    /*应付中期检查*/
     public Vector3[] Move()
    {
        Vector3[] ret = new Vector3[2];
        ret[0] = car.transform.position;
        ret[1] = ret[0] + targetLine.transform.position - car.line.transform.position;
        return ret;
    }
    /* */
    

    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        targetLine = car.line.fatherRoad.lines[targetLineIndex.Value];
        car.linePoints = CalculatePath();
        /*应付中期检查，更换一个换道路径*/
        //car.linePoints = Move();
        /**/
        car.line.cars.Remove(car);
        //行驶路径初始化
        car.line = null;
        car.lineT = 0;
        car.target = car.linePoints[0];
    }

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

    private void End()
    {
        car.changeLine(targetLine);
        car.lineT = Line.CalculateT(car.transform.position, car.line.points);
        car.state = Car.State.inLine;
        car.lineChange = false;
        car.driving();
    }
}
