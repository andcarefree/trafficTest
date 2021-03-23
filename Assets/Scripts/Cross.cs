using UnityEngine;
using System.Collections.Generic;
//路口实例为一个大型的平面透明物体，负责处理车辆在道路之间的调度
//路口类需要根据路口进入车流量对车辆进行概率上的分发（确定车辆进入车道与驶出车道）
//车辆调用链:进入路口区域->选择想要驶出的道路（权重随机）-> 选择能够到达驶出道路的Line并行驶 -> 在目标Road中选择一条目标Line（随机选择或是选择价值系数最优）
//->由所在Line与目标Line算出行驶曲线
//需要注意的是，在路口中并不允许换道超车等行为，于是其中只有跟驰与避让行为，避让可以在车辆前进路线上设置一虚拟车辆使其跟驰。
//车辆流量的设置，采用交叉口流量设置的话，是在每一个RoadIn设置到每一个方向Road的流量，故对于RoadIn->RoadOut的映射需要附带一个流量数据集
public class Cross : MonoBehaviour
{
    /// <summary>
    /// 已经进入路口中的车辆集合
    /// </summary>
    public LinkedList<Car> cars;
    /// <summary>
    /// 道路类与道路包装类的key—value集合
    /// </summary>
    Dictionary<Road, RoadIn> RoadMap;
    /// <summary>
    /// 记录进入路口车辆驶出的目标道路
    /// </summary>
    public Dictionary<Car, Road> carRoadOut;
    /// <summary>
    /// 车辆与路口碰撞题碰撞中触发，道路在路口中时自动初始化
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //与路口碰撞的是道路，需要将该道路映射入路口中
        if (other.gameObject.GetComponent<Road>() != null)
        {
            Road road = other.gameObject.GetComponent<Road>();
            //已经初始化过该道路，快速返回
            if (RoadMap.ContainsKey(road))
            {
                return;
            }
            Vector3 pos = road.lines[0].points[road.lines[0].points.Length - 1];
            //道路尾端接入路口，需要初始化该道路在路口类中的映射
            if (this.gameObject.GetComponent<SphereCollider>().bounds.Contains(pos))
            {
                Dictionary<Road, int> stream = new Dictionary<Road, int>();
                int totalCar = 0;
                foreach (Line line in road.lines)
                {
                    foreach (Road nextRoad in line.nextRoads)
                    {
                        if (stream.ContainsKey(nextRoad))
                        {
                            stream[nextRoad] += 10;
                        }
                        else
                        {
                            stream.Add(nextRoad, 10);
                        }
                    }
                }
                foreach (KeyValuePair<Road, int> rvi in stream)
                {
                    totalCar += rvi.Value;
                }
                RoadMap[road] = new RoadIn(stream, totalCar);
            }
        }
        //车辆与路口碰撞，说明车辆进入路口了，需要为路口中的行驶做准备
        if (other.gameObject.GetComponent<Car>() != null)
        {
            Car car = other.gameObject.GetComponent<Car>();
            //车辆行驶的车道没有下一条路
            if (car.line != null && car.line.nextRoads.Count == 0)
            {
                return;
            }
            //已经驶出路口
            if (car.preCross == this)
            {
                return;
            }
            //判断之前车辆是否已经为驶入路口做好初始化
            if (car.crossLine != null && car.crossLine.Length != 0)
            {
                return;
            }
            //如果车辆将要进入路口时处于转弯状态，暂不初始化
            if (car.state != Car.State.inLine)
            {
                return;
            }
            //车辆即将进入路口，初始化一些数据为车辆路口中的行驶做好准备
            cars.AddLast(car);
            car.state = Car.State.prepareCross;
            car.cross = this;
            return;
        }
    }
    /// <summary>
    /// 车辆进入路口区域触发，为路口中的行驶做好初始化
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        //如果与路口碰撞的不是车辆，跳过该流程
        //其他初始化过程与OnTriggerStay中相同
        if (other.gameObject.GetComponent<Car>() != null) {
            Car car = other.gameObject.GetComponent<Car>();
            if (car.line != null && car.line.nextRoads.Count == 0)
            {
                return;
            }
            cars.AddLast(car);
            car.cross = this;
            if (car.state != Car.State.inLine)
            {
                return;
            }
            car.state = Car.State.prepareCross;
            return;
        }
    }
    /// <summary>
    /// 根据道路车流量权重随机为驶入路口的车辆选择选择驶出道路
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    public Road FindRoadOut(Car car)
    {
        int a = Random.Range(0, 100);
        if (a < 70)
        {
            return car.line.nextRoads[Random.Range(0, car.line.nextRoads.Count)];
        }
        RoadIn roadIn = RoadMap[car.line.fatherRoad];
        float rand = Random.Range(0,roadIn.totalCars);
        int temp = 0;
        foreach (KeyValuePair<Road,int> kvp in roadIn.roadOutStream)
        {
            temp += kvp.Value;
            if (rand <= temp)
            {
                carRoadOut[car] = kvp.Key;
                return kvp.Key;
            }
        }
        Debug.LogError("RoadIn streams set error !");
        return null;
    }
    /// <summary>
    /// 车辆权重随机选择驶出道路后，需要行驶到能够前往驶出道路的车道
    /// </summary>
    /// <param name="car"></param>
    /// <param name="roadOut"></param>
    /// <returns></returns>
    public Line FindLineIn(Car car,Road roadOut)
    {
        Road roadIn = car.line.fatherRoad;
        //循环遍历Car行驶Road中的每一条Line，判断是否能够行驶到roadOut
        for (int i=0; i < roadIn.lines.Length; i++)
        {
            //判断起始位置从当前车道开始
            int j = (i + car.line.indexInRoad()) % roadIn.lines.Length;
            foreach(Road nextRoad in roadIn.lines[j].nextRoads)
            {
                if(nextRoad == roadOut)
                {
                    return roadIn.lines[j];
                }
            }
        }
        Debug.LogError("Cross.FindLineIn error");
        return null;
    }
    /// <summary>
    /// 从已经选择好的roadOut中选择一条“较好”的道路
    /// </summary>
    /// <param name="roadOut"></param>
    /// <param name="linein"></param>
    /// <returns></returns>
    public Line FindLineOut(Road roadOut,Line linein)
    {
        //暂时选择随机选取
        //考虑到同源车辆在路口内避免碰撞的行为，我们让车辆只会行驶到对应道路的半区
        if(linein.indexInRoad() < linein.fatherRoad.lines.Length / 2)
        {
            return roadOut.lines[Random.Range(0, roadOut.lines.Length/2)];
        }
        else
        {
            return roadOut.lines[Random.Range(roadOut.lines.Length/2, roadOut.lines.Length)];
        }
        
    }
    //路口的信号交通配时方案中，考虑到Line末尾的信号灯实际上是控制这一条Line的车辆进入路口的限制
    //而不会分辨驶出道路
    //所以只需要在路口中维护一个Line与其末尾Light的映射集合，Light随着时间变化改变自身状态
    //Line中的车辆只需要在进入路口之前访问Light状态
    //映射关系与Light变化逻辑都放在Cross中，只需要暴露一个根据Line查看Light的接口
    /// <summary>
    /// 维护LineOut对应的交通配时方案
    /// </summary>
    Dictionary<Line, LightTimeSet> LightSet;
    /// <summary>
    /// 维护末端接入路口的Line与Light映射关系
    /// </summary>
    Dictionary<Line, CrossLight> LineLight;
    /// <summary>
    /// 随时间增长的线性值
    /// </summary>
    float LightTime;
    /// <summary>
    /// 该交叉口的信号周期配时
    /// </summary>
    float CycleTime;
    /// <summary>
    /// 向前端暴露设置信号周期方案的接口
    /// </summary>
    /// <param name="line"></param>
    /// <param name="lightSet"></param>
    /// <param name="timeSet"></param>
    public void SetLineLightTime(Line line,List<CrossLight> lightSet, List<float> timeSet)
    {
        LightTimeSet set= new LightTimeSet();
        set.lightSet = lightSet;
        set.timeSet = timeSet;
        LightSet.Add(line, set);
    }
    /// <summary>
    /// 向外暴露获取车道红绿灯状态的接口
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public CrossLight JudgeLight(Line line)
    {
        if (!LineLight.ContainsKey(line))
        {
            Debug.LogError("this line no light");
        }
        return LineLight[line];
    }
    /// <summary>
    /// 初始化路口中的对象
    /// </summary>
    void Start()
    {
        cars = new LinkedList<Car>();
        RoadMap = new Dictionary<Road, RoadIn>();
        carRoadOut = new Dictionary<Car, Road>();
        LightSet = new Dictionary<Line, LightTimeSet>();
        LineLight = new Dictionary<Line, CrossLight>();
    }
    /// <summary>
    /// 控制路口中的红绿灯随时间变化
    /// </summary>
    private void Update()
    {
        LightTime += Time.deltaTime;
        LightTime %= CycleTime;
        //路口中信号灯变化逻辑
        foreach ( KeyValuePair<Line,LightTimeSet> line in LightSet){
            //一个周期内的红绿灯信号变化
            if(LightTime > line.Value.timeSet[line.Value.index])
            {
                line.Value.index++;
                LineLight[line.Key] = line.Value.lightSet[line.Value.index];
            }
            //开启一个新的周期
            if(LightTime < line.Value.timeSet[0] && line.Value.index == line.Value.lightSet.Count)
            {
                line.Value.index = 0;
                LineLight[line.Key] = line.Value.lightSet[line.Value.index];
            }
        }
    }
}
/// <summary>
/// 对接入路口的道路构建一个包装类
/// </summary>
class RoadIn
{
    /// <summary>
    /// 接入路口道路的权重车流
    /// </summary>
    public Dictionary<Road, int> roadOutStream;
    /// <summary>
    /// 总权值
    /// </summary>
    public int totalCars;
    public RoadIn(Dictionary<Road, int> stream, int totalCar)
    {
        this.roadOutStream = stream;
        this.totalCars = totalCar;
    }
}
/// <summary>
/// 红绿灯状态
/// </summary>
public enum CrossLight
{
    RED,
    YELLOW,
    GREEN
}
/// <summary>
/// 红绿灯配时方案
/// </summary>
class LightTimeSet
{
    public List<CrossLight> lightSet;
    public List<float> timeSet;
    public int index;
}
