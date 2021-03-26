using UnityEngine;

// 单个物件的数据类
[System.Serializable]
public class ObjectData
{
    public int id;
    public RoadTypeEnum roadType;
    public Vector3 position;
    public Quaternion rotation;
}
