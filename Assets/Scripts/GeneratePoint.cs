using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePoint : MonoBehaviour
{
    public GameObject car;
    public Car[] cars = new Car[50];
    private int t = 0;
    private Line line;

    //public static GeneratePoint m_generatePoint;

    private float timer = 0;
    private float intervalTime = 1;//生成间隔

    public Car latestCar = null;
    private void Start()
    {
        //m_generatePoint = this;
        line = this.GetComponent<Line>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>=intervalTime )
        {
            GenerateCar();
            timer = 0;
        }
    }
    public void GenerateCar()
    {
        GameObject go = GameObject.Instantiate(car, line.lineStart, Quaternion.identity);
        if (latestCar != null)
            latestCar.behind = go.GetComponent<Car>();
        go.GetComponent<Car>().setLine(line);
        if (t < 50)
        {
            cars[t++] = go.GetComponent<Car>();

        }
    }
}
