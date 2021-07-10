using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class OriginCustom : MonoBehaviour
{
    /// <summary>
    /// 当前的Custom
    /// </summary>
    public static OriginCustom current;
    /// <summary>
    /// 仅仅允许存在一个Custom
    /// </summary>
    public void Start()
    {
        if (current != null)
            Destroy(current.gameObject) ;
        current = this;
    }
    /// <summary>
    /// 跟驰模型委托
    /// </summary>
    /// <param name="m_car"></param> 车辆
    /// <param name="c"></param>
    /// <param name="m"></param>
    /// <param name="l"></param>
    /// <param name="previous"></param>
    /// <returns></returns> 加速度
    public delegate float GM(OCar m_car, float c, float m, float l, OCar previous);

    /// <summary>
    /// 跟驰模型函数
    /// </summary>
    /// <param name="m_car"></param>车辆
    /// <param name="c"></param>
    /// <param name="m"></param>
    /// <param name="l"></param>
    /// <param name="previous"></param>
    /// <returns></returns>加速度
    public virtual float CustomGM(OCar m_car, float c, float m, float l, OCar previous)
    {
        return c * Mathf.Pow(m_car.Km2m(), m) * (previous.Km2m() - m_car.Km2m()) / Mathf.Pow(previous.s - m_car.s, l);
    }

    /// <summary>
    /// 换道路径生成算法委托
    /// </summary>
    /// <param name="car"></param>
    /// <param name="targetLine"></param>
    /// <returns></returns>
    public delegate Vector3[] CP(OCar car, OLine targetLine);
    /// <summary>
    /// 换道路径生成算法
    /// </summary>
    /// <param name="car"></param>
    /// <param name="targetLine"></param>
    /// <returns></returns>
    public virtual Vector3[] CustomCP(OCar car, OLine targetLine)
    {
        Vector3[] ret = new Vector3[4];
        ret[0] = car.transform.position;
        ret[1] = ret[0] + car.transform.forward.normalized * 4.6f;
        ret[2] = ret[1] + targetLine.transform.position - car.line.transform.position;
        ret[3] = ret[2] + ret[1] - ret[0];
        return ret;
    }

    
    public delegate float JV(OCar car, OLine line);
    public virtual float CustomJV(OCar car, OLine line)
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
    
}

