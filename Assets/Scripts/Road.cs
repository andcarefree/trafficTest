using UnityEngine;
public class Road : MonoBehaviour
{
    /// <summary>
    /// 记录道路位置与ID
    /// </summary>
    private int id;
    public int Id { get => id; set => id = value; }
    private Vector3 prevPosition;
    private Vector3 nowPosition;
    public GameObject roadObject;
    /// <summary>
    /// 道路中的车道
    /// </summary>
    public Line[] lines;
    /// <summary>
    /// 标志道路类别
    /// </summary>
    public RoadTypeEnum roadType;
    private ObjectData objectData = new ObjectData();
    /// <summary>
    /// 道路脚本初始化，判断是否为生产车辆的道路，生产唯一ID
    /// </summary>
    private void Start()
    {
        //判断道路是否被选中为产生车辆的起始道路
        if (GetComponent<OriginRoad>() != null)
        {
            roadType = RoadTypeEnum.SOURCE;
        }
        else
        {
            roadType = RoadTypeEnum.NORMAL;
        }
        //为道路生产一个专属ID
        var random = new System.Random();
        id = random.Next(1, int.MaxValue);
        //重置道路ID，将道路加入到一个存储列表
        if (objectData.id == 0)
        {
            objectData.id = id;
            SaveData.current.objects.Add(objectData);
        }
        if (RectangleSelector.current != null)
        {  
            RectangleSelector.current.Selectable.Add(this.gameObject);
        }
        GameEvents.current.OnLoadEvent += DestorySelf;
        GameEvents.current.OnDeleteEvent += DestroySelf;
        //为道路中的所有车道设置父类引用
        lines = GetComponentsInChildren<Line>();
        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }
    private void Update()
    {
        objectData.position = transform.position;
        objectData.rotation = transform.rotation;
        objectData.roadType = roadType;

        foreach (Line line in lines)
        {
            line.fatherRoad = this;
        }
    }
    /// <summary>
    /// 删除道路脚本时调用，更新一些全局资源
    /// </summary>
    private void OnDestroy()
    {
        RectangleSelector.current.Selectable.Remove(this.gameObject);
        SaveData.current.objects.Remove(objectData);
        GameEvents.current.OnLoadEvent -= DestorySelf;
        GameEvents.current.OnDeleteEvent -= DestroySelf;
    }
    /// <summary>
    /// 删除道路预制件接口
    /// </summary>
    private void DestorySelf()
    {
        Destroy(this.gameObject);
    }
    /// <summary>
    /// 通过道路ID删除道路预制件接口
    /// </summary>
    /// <param name="id"></param>
    private void DestroySelf(int id)
    {
        if (id == gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }
}
/// <summary>
/// 道路种类
/// </summary>
public enum RoadTypeEnum 
{ 
    NORMAL, 
    SOURCE 
}