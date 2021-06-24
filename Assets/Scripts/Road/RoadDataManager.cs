using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDataManager : MonoBehaviour
{
    public static List<RoadData> roadDatas;
    private RoadData roadData;

    private void Start()
    {
        if (roadDatas == null)
        {
            roadDatas = new List<RoadData>();
        }

        if (roadData == null)
        {
            roadData = new RoadData();
            roadDatas.Add(roadData);
        }

        roadData.road = this.gameObject;
    }

    private void Update()
    {
        roadData.roadType = this.GetComponent<Road>().roadType;
    }
}
