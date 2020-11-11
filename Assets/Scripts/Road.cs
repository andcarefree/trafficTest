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

    public RoadTypeEnum roadType;

    private void Start()
    {
        lines = GetComponentsInChildren<Line>();
        foreach (Line line in lines) {
            line.fatherRoad = this;
        }
    }
}

public enum RoadTypeEnum
{
    ORIGIN,
    DESTINATION,
    NORMAL
}
