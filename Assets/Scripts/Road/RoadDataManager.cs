using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDataManager : MonoBehaviour
{
    public static List<RoadData> roadDatas;
    private RoadData roadData;

    void Start()
    {
        if (RoadDataManager.roadDatas == null)
        {
            RoadDataManager.roadDatas = new List<RoadData>();
        }

        if (roadData == null)
        {
            roadData = new RoadData();
            RoadDataManager.roadDatas.Add(roadData);
        }

        roadData.roadId = this.gameObject.GetInstanceID();
    }

    void Update()
    {
        roadData.roadType = this.GetComponent<Road>().RoadType;
    }
    
    void OnDestroy()
    {
        RoadDataManager.roadDatas.Remove(roadData);
    }
}
