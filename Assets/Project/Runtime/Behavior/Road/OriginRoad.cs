using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class OriginRoad : MonoBehaviour//泊松分布的交通流
{
    [SerializeField]
    private GameObject[] carPrefab;

    [SerializeField]
    private Road originRoad;

    private float z; // 单位时间内平均到达数（辆/s）
    private float m; // 泊松分布下车辆到达率( m = zt)
    // private float t = 0;
    
    void Start()
    {
        this.originRoad = this.GetComponent<Road>();
        z = 1f;
    }

    void Update()
    {
        /*t += Time.deltaTime;
        m = t * z;*/
        m = Time.deltaTime * z;
        double p = m * Mathf.Exp(-m);
        if (p > UnityEngine.Random.Range(0f, 1f))
        {
            GenerateCar();
            // t = 0;
        }
    }

    private void GenerateCar()
    {
        var line = originRoad.lines[UnityEngine.Random.Range(0, originRoad.lines.Length)];
        var i = (int)UnityEngine.Random.Range(1.0f, 4.0f);

        if (line.cars.Last != null && line.cars.Last.Value.s <= 6.0f)
            return; // 上次同车道生产的车辆未走远时，放弃生产车辆

        var go = GameObject.Instantiate(carPrefab[i], line.lineStart, Quaternion.identity);
        var car = go.GetComponent<Car>();

        car.line = line;
        car.lineT = 0;
        car.linePoints = line.points;
        car.segment = Line.segmentNum;
        line.cars.AddLast(car);
        car.velocity = UnityEngine.Random.Range(20, 30);
    }
}
