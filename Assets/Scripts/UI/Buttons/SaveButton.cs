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
            SaveManager.SaveToJson<LaneData, RoadData>(laneSaveFile, LaneDataManager.laneDatas, RoadDataManager.roadDatas);
        }
    }
    
}
