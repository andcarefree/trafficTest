using UnityEngine;

[System.Serializable]
public class LaneData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public GameObject thisRoad;
    public GameObject[] nextRoad;
}

[System.Serializable]
public class RoadData
{
    public GameObject road; 
    public RoadTypes roadType;
}

[System.Serializable]
public class IntersectionData
{
    public Vector3 position;
    public Vector3 scale;
}
