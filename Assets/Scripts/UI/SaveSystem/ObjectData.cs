using UnityEngine;

[System.Serializable]
public class LaneData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public int thisRoadId;
    public int[] nextRoadId;
}

[System.Serializable]
public class RoadData
{
    public int roadId; 
    public RoadTypes roadType;
}

[System.Serializable]
public class IntersectionData
{
    public Vector3 position;
    public Vector3 scale;
}
