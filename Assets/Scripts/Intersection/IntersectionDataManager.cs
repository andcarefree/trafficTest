using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionDataManager : MonoBehaviour
{
    public static List<IntersectionData> intersectionDatas;
    private IntersectionData intersectionData;

    void Start()
    {
        if (intersectionDatas == null)
        {
            intersectionDatas = new List<IntersectionData>();
        }

        if (intersectionData == null)
        {
            intersectionData = new IntersectionData();
            intersectionDatas.Add(intersectionData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        intersectionData.position = transform.position;
        intersectionData.scale = transform.localScale;
    }
}
