using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 车辆行驶中需要避让的障碍物
/// </summary>
public class Barrier : MonoBehaviour
{

    public Vector3 position;
    public float radius = 4;
    public float s;
    public LinkedList<Car> waitCars;
}
