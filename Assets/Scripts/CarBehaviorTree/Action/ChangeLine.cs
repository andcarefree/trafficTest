using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ChangeLine : Action
{
    Car car;
    Line targetLine;

    /// <summary>
    /// 换道随机选择一个车道
    /// ///TODO 该策略应该是可供二次开发的
    /// </summary>
    private void RandomPick()
    {
        int n = car.line.fatherRoad.lines.Length;
        targetLine = car.line.fatherRoad.lines[Random.Range(0, n)];
        //确保目标路径不为自身
        while(car.line == targetLine)
        {
            targetLine = car.line.fatherRoad.lines[Random.Range(0, n)];
        }
    }

    /// <summary>
    /// 换道路径生成算法
    /// ///TODO 该算法应该是可供二次开发的
    /// </summary>
    private Vector3[] CalculatePath()
    {
        Vector3[] res = new Vector3[4];
        res[0] = car.transform.position;
        res[1] = car.transform.position + 7 * car.transform.forward.normalized;
        res[2] = res[1] + (targetLine.transform.position - car.line.transform.position);
        res[3] = res[2] + 7 * car.transform.forward.normalized;
        return res;
    }
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        car.line.cars.Remove(car);
        RandomPick();
        car.linePoints = CalculatePath();
        //行驶路径初始化
        car.state = Car.State.changing;
        car.line = null;
        car.lineT = 0;
        car.target = car.linePoints[0];
    }

    public override TaskStatus OnUpdate()
    {
        car.accel = 20;
        car.driving();
        if(car.lineT >= 1)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        car.changeLine(targetLine);
        car.lineT = Line.CalculateT(car.transform.position, car.line.points);
        car.state = Car.State.inLine;
        car.lineChange = false;
    }
}
