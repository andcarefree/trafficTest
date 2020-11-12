using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    Vector3 prevPosition;
    Vector3 nowPosition;

    public GameObject roadObject;

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
        prevPosition = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
        ButtonHandler.objectsPosition.Add(prevPosition);
    }
    private void Update()
    {
        nowPosition = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);

        if(nowPosition != prevPosition)
        {
            int index = ButtonHandler.objectsPosition.FindIndex(x => x == prevPosition);
            ButtonHandler.objectsPosition[index] = nowPosition;
            prevPosition = nowPosition;
        }
    }
}

public enum RoadTypeEnum
{
    ORIGIN,
    DESTINATION,
    NORMAL
}
