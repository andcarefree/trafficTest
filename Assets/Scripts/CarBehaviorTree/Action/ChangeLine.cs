using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
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

    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
        car.line.cars.Remove(car);
        RandomPick();
        CalculatePath calculate = new CalculatePath();
        car.linePoints = calculate.ChangePath(car.transform.position, car.transform.forward.normalized, targetLine.transform.position,calculate.MyCalculatePath);
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
        //TODO 应该考虑到T状态的变迁，考虑到换道起始位置与终点位置在初始朝向上的增量，
        //目前这种算法只能适用于直线道路上的换道
        //不用了，数值分析可以直接根据点坐标求其在贝塞尔曲线上的T
        car.lineT = Line.CalculateT(car.transform.position, car.line.points);
        car.state = Car.State.inLine;
        car.lineChange = false;
    }
}
