using System;
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
    public ObjectData objectData;

    private void Start()
    {
        //add the road itself to savelist
        if (objectData.guid == null)
        {
            Debug.Log(SaveData.current.objects);
            SaveData.current.objects.Add(objectData);
        }

        lines = GetComponentsInChildren<Line>();

        if (RectangleSelector.current != null)
            RectangleSelector.current.selectable.Add(this.gameObject);

        GameEvents.current.OnLoadEvent += DestoryOnLoad;

        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }
    private void Update()
    {
        objectData.position = transform.position;
        objectData.rotation = transform.rotation;

        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }

    private void DestoryOnLoad()
    {
        GameEvents.current.OnLoadEvent -= DestoryOnLoad;
        Destroy(this.gameObject);
    }

}

public enum RoadTypeEnum
{
    ORIGIN,
    DESTINATION,
    NORMAL
}
