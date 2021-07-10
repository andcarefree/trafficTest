using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static void SaveToJson<T1, T2, T3>(string path, List<T1> list1, List<T2> list2, List<T3> list3)
    {
        SaveData<T1, T2, T3> saveData = new SaveData<T1, T2, T3>();
        saveData.list1 = list1;
        saveData.list2 = list2;
        saveData.list3 = list3;

        var dataString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, dataString);
    }

    public static (List<T1>, List<T2>, List<T3>) ReadFromJson<T1, T2, T3>(string path)
    {
        var dataString = File.ReadAllText(path);
        var saveData = JsonUtility.FromJson<SaveData<T1, T2, T3>>(dataString);

        return (saveData.list1, saveData.list2, saveData.list3);
    }
}
