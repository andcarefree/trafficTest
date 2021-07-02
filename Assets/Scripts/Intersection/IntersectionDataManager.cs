using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionDataManager : MonoBehaviour
{
    public static List<IntersectionData> intersectionDatas;
    private IntersectionData intersectionData;

    void Start()
    {
        if (IntersectionDataManager.intersectionDatas == null)
        {
            IntersectionDataManager.intersectionDatas = new List<IntersectionData>();
        }

        if (intersectionData == null)
        {
            intersectionData = new IntersectionData();
            IntersectionDataManager.intersectionDatas.Add(intersectionData);
        }
    }

    void Update()
    {
        intersectionData.position = transform.position;
        intersectionData.scale = transform.localScale;
    }

    void OnDestroy()
    {
        IntersectionDataManager.intersectionDatas.Remove(intersectionData);
    }
}
