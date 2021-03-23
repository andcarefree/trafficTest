using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 车道生成车辆预制件的脚本
/// </summary>
public class GeneratePoint : MonoBehaviour
{
    public GameObject car;
    public Car[] cars = new Car[50];
    private int t = 0;
    private Line line;
    public static GeneratePoint m_generatePoint;
    private float timer = 0;
    private float intervalTime = 1;//生成间隔
    public Car latestCar = null;
    /// <summary>
    /// 初始化生产点
    /// </summary>
    private void Start()
    {
        m_generatePoint = this;
        line = this.GetComponent<Line>();
    }
    /// <summary>
    /// 每帧更新计时器
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>=intervalTime )
        {
            GenerateCar();
            timer = 0;
        }
    }
    /// <summary>
    /// 生成车辆方法
    /// </summary>
    public void GenerateCar()
    {
        GameObject go = GameObject.Instantiate(car, line.lineStart, Quaternion.identity);
        go.GetComponent<Car>().setLine(line);
        if (t < 50)
        {
            cars[t++] = go.GetComponent<Car>();

        }
    }
}
