using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneDataManager : MonoBehaviour
{
    public static List<LaneData> laneDatas;
    private LaneData laneData;

    private void Start()
    {
        if (laneDatas == null)
        {
            laneDatas = new List<LaneData>();
        }
        
        if (laneData == null)
        {
            laneData = new LaneData();
            laneDatas.Add(laneData);
        }
    }

    private void Update()
    {
        laneData.position = transform.position;
        laneData.rotation = transform.rotation;
        laneData.scale = transform.localScale;

        Func<Road, GameObject> RoadToGameobject = (road) => 
        {
            return road.gameObject;
        };

        laneData.nextRoad = this.GetComponent<Line>().nextRoads.ConvertAll<GameObject>(new Converter<Road, GameObject>(RoadToGameobject)).ToArray();

        if (transform.parent != null)
        {
            laneData.thisRoad = transform.parent.gameObject;
        }
    }
}
