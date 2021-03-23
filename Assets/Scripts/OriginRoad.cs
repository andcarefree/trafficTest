using UnityEngine;

/// <summary>
/// OriginRoad作为生产车辆的脚本，生成的车辆按照泊松分布
/// </summary>
[DisallowMultipleComponent]
public class OriginRoad : MonoBehaviour//泊松分布的交通流
{
    /// <summary>
    /// 生产对象
    /// </summary>
    public GameObject Car;
    /// <summary>
    /// 附着对象上的road脚本
    /// </summary>
    public Road originRoad;
    private float z;//单位时间内平均到达数（辆/s）
    private float m;//泊松分布下车辆到达率( m = zt)
    private float t = 0;
    /// <summary>
    /// 初始化道路生产脚本
    /// </summary>
    void Start()
    {
        this.originRoad = this.GetComponent<Road>();
        z = 1f;
    }
    /// <summary>
    /// 每一帧按照泊松分布概率生成车辆
    /// </summary>
    void Update()
    {
        /*t += Time.deltaTime;
        m = t * z;*/
        m = Time.deltaTime * z;
        double p = m * Mathf.Exp(-m);
        if(p> UnityEngine.Random.Range(0f,1f))
        {
            GenerateCar();
            //t = 0;
        }
    }
    /// <summary>
    /// 生产车辆预制件的方法，每帧调用
    /// </summary>
    private void GenerateCar()
    {   
        //道路中随机选择一个车道
        Line line = originRoad.lines[UnityEngine.Random.Range(0, originRoad.lines.Length)];
        //上次同车道生产的车辆未走远时，放弃生产车辆
        if (line.cars.Last != null && line.cars.Last.Value.s <= Car.transform.localScale.z)
        {
            return;
        }
        //生成车辆预制件实例
        GameObject go = GameObject.Instantiate(Car, line.lineStart, Quaternion.identity);
        Car car = go.GetComponent<Car>();
        //初始化车辆相关属性
        car.line = line;
        car.lineT = 0;
        car.linePoints = line.points;
        car.segment = Line.segmentNum;
        line.cars.AddLast(car);
        //车辆获得一个随机初速度
        car.velocity = UnityEngine.Random.Range(20, 30);
    }
}
