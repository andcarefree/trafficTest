using UnityEngine;
using System.Collections.Generic;

//路口实例为一个大型的平面透明物体，负责处理车辆在道路之间的调度
//路口类需要根据路口进入车流量对车辆进行概率上的分发（确定车辆进入车道与驶出车道）
//红绿灯处理逻辑也放在该类中
//LineIn增加驶入准许，LineOut增加驶出准许
//红绿灯配时设计
public class Cross : MonoBehaviour
{
    Line[] LineIn;
    Line[] LineOut;
    LinkedList<Car> cars;

    //维护LineOut对应的交通配时方案
    Dictionary<Line, LightTimeSet> LightSet;
    
    //OutLine交通灯状态
    Light[] Light;
    
    //每一个LineOut对应的可驶入的LineIn

    void OnTriggerEnter(Collider other)
    {
        //other is not a car
        if (other.gameObject.GetComponent<Car>() == null) {
            return;
        }
        cars.AddLast(other.gameObject.GetComponent<Car>());
    }

    //按预期车流量分配车辆流向
    //分配之后需要限时转到可换道车辆LineIn
    void discovery()
    {
        
    }

    void Start()
    {
        cars = new LinkedList<Car>();
    }

    void Update()
    {

    }
}

public enum Light
{
    RED,
    YELLOW,
    GREEN
}

class LightTimeSet
{
    float red, yellow, green;
}
