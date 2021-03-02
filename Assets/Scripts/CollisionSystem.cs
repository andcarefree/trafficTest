using UnityEngine;
using System.Collections.Generic;

public static class CollisionSystem
{
    

    //全局冲突域map
    public static Dictionary<KeyValuePair<Line, Line>, Barrier> newbarriers = new Dictionary<KeyValuePair<Line, Line>, Barrier>();
    //路口内的行驶路径
    public static Dictionary<KeyValuePair<Line, Line>, Vector3[]> crossRunPoints = new Dictionary<KeyValuePair<Line, Line>, Vector3[]>();
    //行驶路径与路口匹配
    public static Dictionary<Vector3[], Cross> runPoints2Cross = new Dictionary<Vector3[], Cross>();


    private static Dictionary<KeyValuePair<Car, Car>, Vector3> barriers = new Dictionary<KeyValuePair<Car, Car>, Vector3>();
    //阻碍车辆的障碍物可能有多种，从每一个障碍物都可以获取一个加速度，在所有加速度中取最小值做最小加速度
    private static Dictionary<Car, List<Vector3>> car2barriers = new Dictionary<Car, List<Vector3>>(); 

    private static Car ChooseLucky(Car car1,Car car2)
    {
        //“车辆在车道方向的前后关系”采取一种近似手段
        //即将冲突车辆行驶方向相加得到一个夹角方向，按两车在该夹角方向投影前后判断车辆前后关系
        var targetLine = car1.transform.forward + car2.transform.forward;
        var car1Len = Vector3.Dot(car1.transform.forward, targetLine)/targetLine.magnitude;
        var car2Len = Vector3.Dot(car2.transform.forward, targetLine)/targetLine.magnitude;
        return car1Len > car2Len ? car1 : car2;
    }

    //Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
    //Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
    //same plane, use ClosestPointsOnTwoLines() instead.
    private static Vector3 LineLineIntersection(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);
        Vector3 intersection;
        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parrallel
        if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
        }
        else
        {
            intersection = Vector3.zero;
        }
        return intersection;
    }

    //TODO
    //car1阻碍car2，求出car2可能的barrier
    private static Vector3 GetNewBarrier(Car car1,Car car2)
    {
        if (Vector3.Dot(car1.transform.forward, car2.transform.forward) / car1.transform.forward.magnitude / car2.transform.forward.magnitude <= 5)
        {
            return Vector3.zero;
        }
        //车辆朝向向量的交点即为barrier
        return LineLineIntersection(car1.transform.position,car1.transform.forward,car2.transform.position,car2.transform.forward);
    }
    public static void GetBarrier(Car car1,Car car2)
    {
        var key1 = new KeyValuePair<Car, Car>(car1, car2);
        var key2 = new KeyValuePair<Car, Car>(car2, car1);
        //一对车辆只需要一个车辆让行
        if ( !barriers.ContainsKey(key1) && !barriers.ContainsKey(key2) ){
            //判断两辆车谁先行驶
            //若car2先行，则交换car次序，保证car1先行
            if(ChooseLucky(car1, car2) == car2)
            {
                var temp = car1;
                car1 = car2;
                car2 = temp;
            }
            Vector3 newBarrier = GetNewBarrier(car1, car2);
            //两车行驶方向平行
            if(newBarrier == Vector3.zero)
            {
                return;
            }
            barriers.Add(new KeyValuePair<Car, Car>(car1,car2), newBarrier);
            if (!car2barriers.ContainsKey(car2))
            {
                car2barriers.Add(car2, new List<Vector3>());
            }
            car2barriers[car2].Add(newBarrier);
            return;
        }
    }

    public static void DelBarrier(Car car1,Car car2)
    {
        if( !barriers.ContainsKey(new KeyValuePair<Car, Car>(car1, car2)) ){
            var temp = car1;
            car1 = car2;
            car2 = temp;
        }
        if( !barriers.ContainsKey(new KeyValuePair<Car, Car>(car1, car2)))
        {
            return;
        }
        var delVarriers = barriers[new KeyValuePair<Car, Car>(car1, car2)];
        barriers.Remove(new KeyValuePair<Car, Car>(car1, car2));
        /*if (car2barriers.ContainsKey(car2))
        {*/
            car2barriers[car2].Remove(delVarriers);
        //}
    }
    public static float ToGiveWay(Car car)
    {
        //test
        if(car.state != Car.State.crossing)
        {
            return car.accel;
        }

        float ret = car.accel;
        if( car2barriers.ContainsKey(car))
        {
            foreach (Vector3 barrier in car2barriers[car])
            {
                ret = Mathf.Min(ret, Mathf.Pow(car.Km2m(), 1.5f) * (-car.Km2m()) / Mathf.Pow(Vector3.Distance(barrier, car.transform.position), 1));
            }
        }
        return ret;
    }
}
