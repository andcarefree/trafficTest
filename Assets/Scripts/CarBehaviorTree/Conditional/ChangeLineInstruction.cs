using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
/// <summary>
/// 车辆是否换道的判断脚本
/// </summary>
public class ChangeLineInstruction : Conditional
{
    Car car;
    /// <summary>
    /// 限制车辆换道频率
    /// </summary>
    float changTime;
    public SharedInt targetLineIndex;
    /// <summary>
    /// 为车道行驶价值系数提供二次开发接口
    /// </summary>
    public static OriginCustom.JV jv=JudgeValue;
    /// <summary>
    /// 判断换道产生的收益值,返回值[0,1]
    /// </summary>
    /// <param name="car"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static float JudgeValue(OCar car, OLine line)
    {
        //目标车道不存在车辆
        if (line.cars.First == null)
        {
            return 1;
        }
        OCar near = car.CarClosest(line);
        float PreSpeed;//间隙前车速度
        float nextS;//间隙后车的行驶间距
        float preS;//间隙前车的行驶间距
        //near在car之后
        if (OCar.judgeLocation(car, near))
        {
            //换道之后成为头车且间隙允许换道
            if (near.PreCar() == null)
            {
                preS = 10000;
                PreSpeed = 10000;
            }
            else
            {
                OCar nearPre = near.PreCar();//间隙前车
                PreSpeed = nearPre.velocity;
                preS = nearPre.s - car.s;
            }
            nextS = car.s - near.s;
        }
        else//near在car之前
        {
            OCar nearNext = near.NextCar();
            PreSpeed = near.velocity;
            preS = near.s - car.s;
            if (nearNext == null)
            {
                nextS = 10000;//设立一个很大的数表示无穷
            }
            else
            {
                nextS = car.s - nearNext.s;
            }

        }
        if (preS <= car.transform.localScale.z || nextS <= car.transform.lossyScale.z * 1.5)
        {
            return 0;
        }
        if (car.PreCar() == null || (PreSpeed <= car.PreCar().velocity && preS - car.PreCar().s < car.transform.lossyScale.z))
        {
            return 0;
        }
        return 1;
    }
    /// <summary>
    /// 车道选择策略
    /// 从该车道临近车道选择一个正收益车道
    /// </summary>
    private Line NearLinePick()
    {
        Line[] lines = car.line.fatherRoad.lines;
        int nowIndex = car.line.indexInRoad();
        if (nowIndex == 0)
        {
            return jv(car, lines[1]) > 0 ? lines[1] : car.line;
        }
        else if (nowIndex == lines.Length - 1)
        {
            return jv(car, lines[lines.Length - 2]) > 0 ? lines[lines.Length - 2] : car.line;
        }
        else
        {
            if (jv(car, lines[nowIndex - 1]) > 0)
            {
                return lines[nowIndex - 1];
            }
            else if (jv(car, lines[nowIndex + 1]) > 0)
            {
                return lines[nowIndex + 1];
            }
            else
            {
                return car.line;//选择本身
            }
        }
    }
    /// <summary>
    /// 选择左侧车道换道
    /// </summary>
    /// <returns></returns>
    private Line LeftPick()
    {
        Line[] lines = car.line.fatherRoad.lines;
        int nowIndex = car.line.indexInRoad();
        if (nowIndex == 0)
        {
            return car.line;
        }
        return jv(car, lines[nowIndex - 1]) > 0 ? lines[nowIndex - 1] : car.line;
    }
    /// <summary>
    /// 选择右侧车道换道
    /// </summary>
    /// <returns></returns>
    private Line RightPick()
    {
        Line[] lines = car.line.fatherRoad.lines;
        int nowIndex = car.line.indexInRoad();
        if (nowIndex == lines.Length - 1)
        {
            return car.line;
        }
        return jv(car, lines[nowIndex + 1]) > 0 ? lines[nowIndex + 1] : car.line;
    }
    /// <summary>
    /// 车辆不满足当前行驶环境时寻求主动换道时修改car.lineChange
    /// </summary>
    private void PositiveChange()
    {
        //换道条件判断
        if (car.line != null && car.line.cars.First != null && car.line.cars.First.Value != car && car.s > car.transform.localScale.z)
        {
            if (car.line.cars.Find(car) == null)
            {
                return;
            }
            Car pre = (Car)car.line.cars.Find(car).Previous.Value;
            //当前车道运行速度低于预期时，寻求车道换道
            if ((pre.velocity <= 40 && car.expectVelocity - car.velocity > 5 && car.accel < 5) || (pre.velocity >= 40 && car.expectVelocity - car.velocity > 15 && car.accel < 5))
            {
                car.lineChange = true;
            }
        }
    }
    /// <summary>
    /// 换道目标不为自身，正式开始换道
    /// </summary>
    /// <returns></returns>
    private bool SuccessChange()
    {
        Line line = PickLine();
        if (!line.Equals(car.line) && CanPreGap(car,line) && CanNextGap(car,line))
        {
            targetLineIndex.Value = line.indexInRoad();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 选择一个行驶价值最高的车道
    /// </summary>
    /// <returns></returns>
    private Line PickLine()
    {
        if(JudgeRightValue(car) >= JudgeLeftValue(car))
        {
            if(JudgeLineValue(car) >= JudgeRightValue(car)) {
                return car.line;
            }
            else
            {
                return car.line.fatherRoad.lines[car.line.indexInRoad() + 1];
            }
        }
        else
        {
            if(JudgeLineValue(car) >= JudgeLeftValue(car))
            {
                return car.line;
            }
            else
            {
                return car.line.fatherRoad.lines[car.line.indexInRoad() - 1];
            }
        }
    }
    /// <summary>
    /// 左侧车道价值: 0.57-0.32V1
    /// V1: 决策车辆与左车道后车相对速度
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    private static double JudgeLeftValue(Car car)
    {
        if (car.line.indexInRoad() == 0)//当前车道为最左车道
        {
            return double.MinValue;
        }
        var LeftLine = car.line.fatherRoad.lines[car.line.indexInRoad() - 1];
        var cars = LeftLine.cars;
        var nearCarV = 0.0;
        foreach (var nearCar in cars)
        {
            if (((Car)nearCar).s < car.s)
            {
                nearCarV = ((Car)nearCar).velocity;
            }
        }
        return 0.57 - 0.32 * (car.velocity - nearCarV);
    }
    /// <summary>
    /// 当前车道价值: 0.28*V2 + 0.36*V3 + 0.21S
    /// V2：目标车与前车相对速度
    /// V3：目标车与后车相对速度
    /// S：目标车与当前车道前车相对位置
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    private static double JudgeLineValue(Car car)
    {
        var preV = double.MaxValue;
        var preS = double.MaxValue;
        if (car.PreCar() != null)
        {
            preV = car.PreCar().velocity;
            preS = car.PreCar().s;
        }
        var nextV = 0.0;
        if (car.NextCar() != null)
        {
            nextV = car.NextCar().velocity;
        }
        return 0.28 * (preV - car.velocity) + 0.36 * (car.velocity - nextV) + 0.21 * (preS - car.s);
    }
    /// <summary>
    /// 右侧车道价值: 0.17 - 0.22*V4
    /// V4 ：决策车辆与右车道后车相对速度
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    private static double JudgeRightValue(Car car)
    {
        if (car.line.indexInRoad() == car.line.fatherRoad.lines.Length - 1)//当前车道为最右车道
        {
            return double.MinValue;
        }
        var RightLine = car.line.fatherRoad.lines[car.line.indexInRoad() + 1];
        var cars = RightLine.cars;
        var nearCarV = 0.0;
        foreach (var nearCar in cars)
        {
            if (((Car)nearCar).s < car.s)
            {
                nearCarV = ((Car)nearCar).velocity;
            }
        }
        return 0.17 - 0.22 * (car.velocity - nearCarV);
    }
    /// <summary>
    /// 判断前车间隙是否能够换道
    /// 换道临界前车间隙： G1 = exp{1.23 - 0.34*max(0,V5) - 0.21*min(0,V5)}
    /// V5: 与目标车道前车相对速度
    /// </summary>
    /// <param name="car"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    private static bool CanPreGap(Car car, Line line)//上述公式条件过于严苛，尝试放宽条件
    {
        //目标车道没有前车
        if (line.cars.First == null || line.cars.First.Value.s < car.s)
        {
            return true;
        }
        foreach (var node in line.cars)
        {
            var preCar = (Car)node;
            //找到前车
            if (preCar.NextCar() == null || preCar.NextCar().s < car.s)
            {
                var V5 = preCar.velocity - car.velocity;
                var G1 = Mathf.Exp((float)(1.23 - 0.34 * Mathf.Max(0, V5) - 0.21 * Mathf.Min(0, V5)));
                //尝试缩小临界前车间隙
                return preCar.s - car.s >= G1/2;
            }
        }
        return true;
    }
    /// <summary>
    /// 换道临界后车间隙： G2 = exp{1.35 - 0.41*max(0,V6) - 0.28*min(0,V6)}
    /// V6: 与目标车道后车相对速度
    /// </summary>
    /// <param name="car"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    private static bool CanNextGap(Car car, Line line)
    {
        foreach (var node in line.cars)
        {
            var nextCar = (Car)node;
            //找到前车
            if (nextCar.s < car.s)
            {
                var V6 = car.velocity - nextCar.velocity;
                var G2 = Mathf.Exp((float)(1.35 - 0.41 * Mathf.Max(0, V6) - 0.28 * Mathf.Min(0, V6)));
                return car.s - nextCar.s >= G2/2;
            }
        }
        return true;
    }
    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }
    /// <summary>
    /// 每一帧都判断当前车辆是否需要换道
    /// </summary>
    /// <returns></returns>
    public override TaskStatus OnUpdate()
    {
        if (car.state == Car.State.prepareCross)
        {
            return TaskStatus.Failure;
        }
        //将要进入路口时就禁止主动换道
        if (car.cross != null && car.state == OCar.State.inLine)
        {
            return TaskStatus.Failure;
        }
        if (car.state == Car.State.changing)
        {
            //changTime = 0;
            return TaskStatus.Success;
        }
        /*
        if (changTime < 1)
        {
            changTime += Time.deltaTime;
            return TaskStatus.Failure;
        }*/

        //PositiveChange();

        /*if (car.lineChange == true)
        {*/
            //即将驶入路口，不予换道
            if (car.state == Car.State.prepareCross)
            {

                return TaskStatus.Failure;
            }
            if (SuccessChange())
            {
            Debug.LogWarning("count");
                car.state = Car.State.changing;
                return TaskStatus.Success;
            }
        /*}*/
        /*//最终判断不支持换道，驳回换道请求
        car.lineChange = false;*/
        return TaskStatus.Failure;
    }
}
