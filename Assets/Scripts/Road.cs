using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private int id;
    public int Id { get => id; set => id = value; }

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
        // 确认该道路是不是车辆源
        if (GetComponent<OriginRoad>() != null)
        {
            roadType = RoadTypeEnum.SOURCE;
        }
        else
        {
            roadType = RoadTypeEnum.NORMAL;
        }

        // 给道路设置id
        var random = new System.Random();
        id = random.Next(1, int.MaxValue);

        // 添加到saveList中，供存读数据用
        if (objectData.id == 0)
        {
            objectData.id = id;
            SaveData.current.objects.Add(objectData);
        }

        if (Selector.current != null)
        {  
            Selector.current.Selectable.Add(this.gameObject);
        }

        GameEvents.current.OnLoadEvent += DestorySelf;
        GameEvents.current.OnDeleteEvent += DestroySelf;

        lines = GetComponentsInChildren<Line>();

        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }

    private void Update()
    {
        objectData.position = transform.position;
        objectData.rotation = transform.rotation;
        objectData.roadType = roadType;

        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }

    private void OnDestroy()
    {
        Selector.current.Selectable.Remove(this.gameObject);
        SaveData.current.objects.Remove(objectData);
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
    NORMAL, 
    SOURCE 
}