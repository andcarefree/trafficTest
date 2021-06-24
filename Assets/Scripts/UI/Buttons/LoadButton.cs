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
    private GameObject warningPanel;
    
    [SerializeField]
    private TextMeshProUGUI warningText;

    public void OnLoad()
    {
        var loadFile = FileDialog.OpenFileDialog("打开道路存档", "JSON 源文件 (.json)", "*.json");
        
        if (loadFile != null)
        {
            var tuple = SaveManager.ReadFromJson<LaneData, RoadData>(loadFile);

            var oldRoad = GameObject.FindGameObjectsWithTag("Road");
            for (int i = 0; i < oldRoad.Length; i++)
            {
                Destroy(oldRoad[i]);
            }

            LaneDataManager.laneDatas = tuple.Item1;
            RoadDataManager.roadDatas = tuple.Item2;

            var newRoadDict = new Dictionary<GameObject, GameObject>();

            foreach (var roadData in RoadDataManager.roadDatas)
            {
                var newRoad = Instantiate(roadPrefab);
                newRoadDict.Add(roadData.road, newRoad);

                if (roadData.roadType == RoadTypes.SOURCE)
                {
                    newRoad.GetComponent<OriginRoad>().enabled = true;
                }
            }

            foreach (var laneData in LaneDataManager.laneDatas)
            {
                var newLane = Instantiate(lanePrefab, laneData.position, laneData.rotation);

                newLane.transform.SetParent(newRoadDict[laneData.thisRoad].transform);
                newLane.transform.localScale = laneData.scale;

                foreach (var nextRoad in laneData.nextRoad)
                {
                    newLane.GetComponent<Line>().nextRoads.Add(newRoadDict[nextRoad].GetComponent<Road>());
                }
            }
        }
    }
}
