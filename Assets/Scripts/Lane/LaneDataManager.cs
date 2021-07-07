using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneDataManager : MonoBehaviour
{
    public static List<LaneData> laneDatas;
    private LaneData laneData;

    void Start()
    {
        if (LaneDataManager.laneDatas == null)
        {
            LaneDataManager.laneDatas = new List<LaneData>();
        }
        
        if (laneData == null)
        {
            laneData = new LaneData();
            LaneDataManager.laneDatas.Add(laneData);
        }
    }

    void Update()
    {
        laneData.position = transform.position;
        laneData.rotation = transform.rotation;
        laneData.scale = transform.localScale;
        laneData.lightInfos = GetComponent<Line>().lightInfos;

        Func<Road, int> RoadToId = (road) => 
        {
            return road.gameObject.GetInstanceID();
        };

        laneData.nextRoadId = GetComponent<Line>().nextRoads.ConvertAll<int>(new Converter<Road, int>(RoadToId));

        if (transform.parent != null)
        {
            laneData.thisRoadId = transform.parent.gameObject.GetInstanceID();
        }
    }

    void OnDestroy()
    {
        LaneDataManager.laneDatas.Remove(laneData);
    }
}
