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

    void Start()
    {
        lines = GetComponentsInChildren<Line>();

        foreach (Line line in lines)
            line.fatherRoad = this;

        GameEvents.Instance.OnLoadEvent += DestroySelfOnLoad;
    }

    void Update()
    {
        foreach (Line line in lines)
            line.fatherRoad = this;

        lines = GetComponentsInChildren<Line>();
        
        if (lines.Length == 0)
            Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        GameEvents.Instance.OnLoadEvent -= DestroySelfOnLoad;
    }

    private void DestroySelfOnLoad()
    {
        Destroy(this.gameObject);
    }
}

public enum RoadTypes
{ 
    NORMAL, 
    SOURCE 
}