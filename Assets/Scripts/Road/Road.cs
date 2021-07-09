using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Road : MonoBehaviour
{
    /// <summary>
    /// 道路
    /// </summary>
    [field : SerializeField]
    public Line[] lines { get; set; }

    /// <summary>
    /// 道路是否允许通行
    /// </summary>
    public RoadTypes RoadType { get; set; }

    void Awake()
    {
        GameEvents.Instance.OnLoadEvent += DestroySelfOnLoad;
        GameEvents.Instance.OnLateLoadEvent += ArrangeLane;
        GameEvents.Instance.OnRoadCreateEvent += ArrangeLane;
    }

    void Update()
    {
        if (lines != null)
        {
            foreach (var line in lines)
            line.fatherRoad = this;
        }
        
        if (lines.Length == 0)
        {
            Destroy(this.gameObject);
        }
        
        #if UNITY_EDITOR
        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log($"{i}, {lines[i].gameObject.GetInstanceID()}");
        }
        #endif
    }

    // unregister events on destroy
    void OnDestroy()
    {
        GameEvents.Instance.OnLoadEvent -= DestroySelfOnLoad;
        GameEvents.Instance.OnLateLoadEvent += ArrangeLane;
        GameEvents.Instance.OnRoadCreateEvent -= ArrangeLane;
    }

    private void DestroySelfOnLoad()
    {
        Destroy(this.gameObject);
    }

    // Arrange lanes
    // for the lane changing decision has dependence on the order
    private void ArrangeLane(int id)
    {
        #if UNITY_EDITOR
        Debug.Log("ArrangeLane() is called");
        #endif

        if (id == gameObject.GetInstanceID()) 
        {
            ArrangeLane();
        }   
    }

    private void ArrangeLane()
    {
        #if UNITY_EDITOR
        Debug.Log($"Arranging lanes, Road ID is {gameObject.GetInstanceID()}");
        #endif

        // use projection to convert Vector3 into float
        lines = GetComponentsInChildren<Line>();

        var direction = lines[0].transform.rotation * Vector3.back;
        var laneDictionary = new Dictionary<float, Line>();
        
        foreach (var lane in lines)
        {
            var lanePosition = lane.transform.position;
            var laneProjection = Vector3.Dot(lanePosition, direction);

            #if UNITY_EDITOR
            Debug.Log($"{laneProjection}, {lane.gameObject.GetInstanceID()}");
            #endif

            laneDictionary.Add(laneProjection, lane);
        }

        // using LINQ to reorder lanes
        lines = laneDictionary.OrderBy(x => x.Key).Select(x => x.Value).ToArray();

        #if UNITY_EDITOR
        int i = 0;
        foreach (var lane in lines)
        {
            Debug.Log($"{i++}, {lane.gameObject.GetInstanceID()}");
        }
        #endif

    }
}

public enum RoadTypes
{ 
    NORMAL, 
    SOURCE 
}