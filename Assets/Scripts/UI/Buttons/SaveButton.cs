using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButton : MonoBehaviour
{
    [SerializeField]
    private GameObject warningPanel;

    [SerializeField]
    private TextMeshProUGUI warningText;

    public void OnSave()
    {
        var laneSaveFile = FileDialog.SaveFileDialog("保存车道信息", "JSON 源文件 (.json)", "*.json");

        if (laneSaveFile != null)
        {
            var list1 = LaneDataManager.laneDatas;
            var list2 = RoadDataManager.roadDatas;
            var list3 = IntersectionDataManager.intersectionDatas;

            SaveManager.SaveToJson<LaneData, RoadData, IntersectionData>(laneSaveFile, list1, list2, list3);
        }
    }
    
}
