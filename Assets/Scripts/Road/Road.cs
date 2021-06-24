using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    /// <summary>
    /// 道路
    /// </summary>
    public Line[] lines;

    /// <summary>
    /// 道路是否允许通行
    /// </summary>
    public RoadTypes roadType;

    private void Start()
    {
        lines = GetComponentsInChildren<Line>();

        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }

    private void Update()
    {
        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }

}

public enum RoadTypes
{ 
    NORMAL, 
    SOURCE 
}