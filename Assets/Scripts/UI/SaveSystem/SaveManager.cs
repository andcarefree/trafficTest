using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static void SaveToJson<T1, T2>(string path, List<T1> list1, List<T2> list2)
    {
        SaveData<T1, T2> saveData = new SaveData<T1, T2>();
        saveData.list1 = list1;
        saveData.list2 = list2;

        var dataString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, dataString);
    }

    public static Tuple<List<T1>, List<T2>> ReadFromJson<T1, T2>(string path)
    {
        var dataString = File.ReadAllText(path);
        var saveData = JsonUtility.FromJson<SaveData<T1, T2>>(dataString);

        return new Tuple<List<T1>, List<T2>>(saveData.list1, saveData.list2);
    }
}
