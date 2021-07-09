using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    [SerializeField]
    private GameObject lanePrefab;

    [SerializeField]
    private GameObject roadPrefab;

    [SerializeField]
    private GameObject intersectionPrefab;

    [SerializeField]
    private GameObject warningPanel;
    
    [SerializeField]
    private TextMeshProUGUI warningText;

    public void OnLoad()
    {
        var loadFile = FileDialog.OpenFileDialog("打开道路存档", "JSON 源文件 (.json)", "*.json");
        
        if (loadFile != null)
        {
            var tuple = SaveManager.ReadFromJson<LaneData, RoadData, IntersectionData>(loadFile);
            var oldRoad = GameObject.FindGameObjectsWithTag("Road");
            var oldIntersection = GameObject.FindGameObjectsWithTag("Intersection");
            var newRoadDict = new Dictionary<int, GameObject>();

            GameEvents.Instance.OnLoad();

            var laneDatas = tuple.Item1;
            var roadDatas = tuple.Item2;
            var intersectionDatas = tuple.Item3;

            foreach (var roadData in roadDatas)
            {
                var newRoad = Instantiate(roadPrefab);
                newRoadDict.Add(roadData.roadId, newRoad);

                if (roadData.roadType == RoadTypes.SOURCE)
                {
                    newRoad.GetComponent<OriginRoad>().enabled = true;
                    newRoad.GetComponent<Road>().RoadType = RoadTypes.SOURCE;
                }
            }

            foreach (var laneData in laneDatas)
            {
                var newLane = Instantiate(lanePrefab, laneData.position, laneData.rotation);

                if (laneData.thisRoadId != 0)
                {
                    newLane.transform.SetParent(newRoadDict[laneData.thisRoadId].transform);
                }

                newLane.transform.localScale = laneData.scale;

                foreach (var nextRoad in laneData.nextRoadId)
                {
                    newLane.GetComponent<Line>().nextRoads.Add(newRoadDict[nextRoad].GetComponent<Road>());
                }

                newLane.GetComponent<Line>().lightInfos = laneData.lightInfos;
            }

            foreach (var intersectionData in intersectionDatas)
            {
                var newIntersection = Instantiate(intersectionPrefab, intersectionData.position, Quaternion.identity);
                
                newIntersection.transform.localScale = intersectionData.scale;
            }
        }
    }
}
