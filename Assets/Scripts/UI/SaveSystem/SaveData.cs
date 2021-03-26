using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 所有物件的数据集合，单例
[System.Serializable]
public class SaveData
{
    private static SaveData _current;
    public static SaveData current
    {
        get
        {
            if(_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            if(value != null)
            {
                _current = value;
            }
        }
    }
    public List<ObjectData> objects = new List<ObjectData>();
}
