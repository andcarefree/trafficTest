using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private Vector3 prevPosition;
    private Vector3 nowPosition;
    public GameObject roadObject;

    /// <summary>
    /// 道路
    /// </summary>
    public Line[] lines;

    /// <summary>
    /// 道路是否允许通行
    /// </summary>
    public RoadTypeEnum roadType;
    private ObjectData objectData = new ObjectData();

    private void Start()
    {
        //add the road itself to savelist
        if (objectData.guid == null)
        {
            SaveData.current.objects.Add(objectData);
        }

        lines = GetComponentsInChildren<Line>();

        if (RectangleSelector.current != null)
        {  
            RectangleSelector.current.Selectable.Add(this.gameObject);
        }

        GameEvents.current.OnLoadEvent += DestorySelf;
        GameEvents.current.OnDeleteEvent += DestroySelf;

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

    void OnDestroy()
    {
        RectangleSelector.current.Selectable.Remove(this.gameObject);
        GameEvents.current.OnLoadEvent -= DestorySelf;
        GameEvents.current.OnDeleteEvent -= DestroySelf;
    }

    private void DestorySelf()
    {
        Destroy(this.gameObject);
    }

    private void DestroySelf(int id)
    {
        if (id == gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }
}

public enum RoadTypeEnum
{
    ORIGIN,
    DESTINATION,
    NORMAL
}
