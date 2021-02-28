using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

//通过改变car.lineChange发出对车辆的换道指令
public class ChangeLineInstruction : Conditional
{
    Car car;

    //限制换道频率
    float changTime;

    public SharedInt targetLineIndex;
    public static OriginCustom.JV jv=JudgeValue;
    /// <summary>
    /// 判断换道产生的收益值,返回值[0,1]
    /// </summary>
    //TODO 现在只考虑双值，0代表不值得换道，1代表值得换道
    //现在需要考虑的因素：1.换道间隙够大
    //                2.间隙之前的车够快或够远
    //                3.间隙之后的车够远
    //TODO 对外提供收益函数的替换接口
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

    /*应付中期*/
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
    /**/


    /// <summary>
    /// 车辆不满足当前行驶环境时寻求主动换道时修改car.lineChange
    /// </summary>
    private void PositiveChange()
    {
        //换道条件判断
        if (car.line != null && car.line.cars.First != null && car.line.cars.First.Value != car && car.s > car.transform.localScale.z)
        {
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
        Line line = NearLinePick();
        /*应付中期*/
        //Line line = RightPick();
        //Line line = LeftPick();
        /**/
        if (!line.Equals(car.line))
        {
            targetLineIndex.Value = line.indexInRoad();
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
        car = gameObject.GetComponent<Car>();
    }

    public override TaskStatus OnUpdate()
    {
        if (car.state == Car.State.prepareCross)
        {
            return TaskStatus.Failure;
        }
        if (car.crossLine != null && car.crossLine.Length != 0)
        {
            return TaskStatus.Failure;
        }
        if (car.state == Car.State.changing)
        {
            changTime = 0;
            return TaskStatus.Success;
        }

        if (changTime < 1)
        {
            changTime += Time.deltaTime;
            return TaskStatus.Failure;
        }

        PositiveChange();

        if (car.lineChange == true)
        {
            //即将驶入路口，不予换道
            if (car.state == Car.State.prepareCross)
            {

                return TaskStatus.Failure;
            }
            if (SuccessChange() == true)
            {
                car.state = Car.State.changing;
                return TaskStatus.Success;
            }
        }
        //最终判断不支持换道，驳回换道请求
        car.lineChange = false;
        return TaskStatus.Failure;
    }
}
