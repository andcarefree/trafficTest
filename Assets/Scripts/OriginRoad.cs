using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class OriginRoad : MonoBehaviour//泊松分布的交通流
{
    public GameObject Car;
    public Road originRoad;
    private float z;//单位时间内平均到达数（辆/s）
    private float m;//泊松分布下车辆到达率( m = zt)
    private float t = 0;
    // Use this for initialization
    void Start()
    {
        originRoad = gameObject.GetComponent<Road>();
        z = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        /*t += Time.deltaTime;
        m = t * z;*/
        m = Time.deltaTime * z;
        double p = m * Mathf.Exp(-m);
        if(p> UnityEngine.Random.Range(0f,1f))
        {
            GenerateCar();
            //t = 0;
        }
    }

    private void GenerateCar()
    {
        Line line = originRoad.lines[UnityEngine.Random.Range(0, originRoad.lines.Length)];
        if (line.lastCar != null&& line.lastCar.s <= 2) return;//上次同车道生产的车辆未走远时，放弃生产车辆
        GameObject go = GameObject.Instantiate(Car, line.lineStart, Quaternion.identity);
        go.GetComponent<Car>().setLine(line);
    }
}
