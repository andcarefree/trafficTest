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
        if (string.IsNullOrEmpty(objectData.id))
        {
            objectData.id = System.DateTime.Now.ToLongDateString() + System.DateTime.Now.ToLongTimeString() + Random.Range(0, int.MaxValue).ToString();
            
            SaveData.current.objects.Add(objectData);
        }
        
        lines = GetComponentsInChildren<Line>();

        RectangleSelector.current.selectable.Add(this.gameObject);

        GameEvents.current.OnLoadEvent += DestoryOnLoad;

        // prevPosition = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
        // ButtonHandler.objectsPosition.Add(prevPosition);
        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }
    private void Update()
    {
        objectData.position = transform.position;
        objectData.rotation = transform.rotation;

        //
        // nowPosition = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
        //
        // if(nowPosition != prevPosition)
        // {
        //     int index = ButtonHandler.objectsPosition.FindIndex(x => x == prevPosition);
        //     ButtonHandler.objectsPosition[index] = nowPosition;
        //     prevPosition = nowPosition;
        // }
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
