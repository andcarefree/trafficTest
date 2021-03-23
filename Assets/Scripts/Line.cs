using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Line : OLine
{
    /// <summary>
    /// 车道渲染器
    /// </summary>
    private LineRenderer lineRenderer;
    /// <summary>
    /// 车道的贝塞尔控制点
    /// </summary>
    public Vector3[] points;
    /// <summary>
    /// 车道离散化的轨迹点个数
    /// </summary>
    public const int segmentNum = 100;
    /// <summary>
    /// 车道所属道路
    /// </summary>
    public Road fatherRoad;
    /// <summary>
    /// 车道限速
    /// </summary>
    public float maxVelocity;
    public Car lineLock;
    /// <summary>
    /// 车道所能通向的下一条道路
    /// </summary>
    public List<Road> nextRoads;
    public Vector3 lineStart { get => points[0]; }
    public Vector3 lineEnd { get => points[points.Length -1]; }
    public Vector3 startVector{ get => (points[1] - points[0]).normalized; }
    public Vector3 endVector { get => (points[points.Length - 1] - points[points.Length - 2]).normalized; }
    /// <summary>
    /// 获取车道在道路中的下标
    /// </summary>
    /// <returns></returns>
    public int indexInRoad()
    {
        for (int i = 0; i < fatherRoad.lines.Length; i++)
        {
            if (fatherRoad.lines[i] == this)
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// 初始化车道相关字段
    /// </summary>
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerID = 0;
        cars = new LinkedList<OCar>();
        maxVelocity = 70;

        if (RectangleSelector.current != null)
        {
            RectangleSelector.current.Selectable.Add(this.gameObject);
        }
    }
    /// <summary>
    /// 渲染车道，使其可见
    /// </summary>
    private void Update()
    {
        //获取子点对象的transform
        Transform[] pointTran = GetComponentsInChildren<Transform>();
        points = new Vector3[pointTran.Length - 1];
        for (int i = 1; i < pointTran.Length; i++)
        {
            //获取子点对象的坐标
            points[i - 1] = pointTran[i].position;
        }
        DrawCurve();
    }
    private void OnDestroy()
    {
        RectangleSelector.current.Selectable.Remove(this.gameObject);
        GameEvents.current.OnDeleteEvent -= DestroySelf;
    }
    private void DestroySelf(int id)
    {
        if (id == gameObject.GetInstanceID())
        {
            RectangleSelector.current.Selectable.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// 根据车道的贝塞尔控制点绘制车道
    /// </summary>
    private void DrawCurve()
    {
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = (float)i / (float)segmentNum;
            Vector3 pixel = Bezier(t, points);
            lineRenderer.positionCount = i;
            lineRenderer.SetPosition(i - 1, pixel);
        }
    }
    /// <summary>
    /// 根据贝塞尔曲线控制点与T值得到轨迹中的点
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static Vector3 Bezier(float t, Vector3[] p)
    {
        Vector3 ans = Vector3.zero;
        int n = p.Length;
        for (int i = 0; i < n; i++)
        {
            ans += p[i] * C(i, n - 1) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - 1 - i);
        }
        return ans;
    }
    /// <summary>
    /// 求出点在贝塞尔曲线上对应的T值，简单二分法
    /// </summary>
    public static float CalculateT(Vector3 point, Vector3[] p)
    {
        float start = 0f;
        float end = 1f;
        float mid = (start + end) / 2;
        while (Vector3.Distance(point, Bezier(mid, p)) >= 2f)
        {
            if (Vector3.Distance(point, Bezier(start, p)) > Vector3.Distance(point, Bezier(end, p)))
            {
                start = mid + 0.01f;
            }
            else
            {
                end = mid;
            }
            mid = (start + end) / 2;
        }
        return mid;
    }
    /// <returns>
    /// n! / m!(n-m)!
    /// </returns>
    private static int C(int m, int n)
    {
        int ans = 1;
        for (int i = 1; i <= n; i++)
            ans = ans * i;
        for (int i = 1; i <= m; i++)
            ans = ans / i;
        for (int i = 1; i <= n - m; i++)
            ans = ans / i;
        return ans;
    }
    /// <summary>
    /// 两条Line之间自动生成一条平滑曲线，以路点形式返回
    /// </summary>
    public static Vector3[] Interpolation(Line line1, Line line2)
    {
        Vector3[] ans = new Vector3[3];
        float b = ((line2.lineStart.x - line1.lineEnd.x) * line1.endVector.z
                  - (line2.lineStart.z - line1.lineEnd.z) * line1.endVector.x) / (line2.startVector.z * line1.endVector.x - line2.startVector.x * line1.endVector.z);
        //float a = (line2.lineStart.x - line1.lineEnd.y + b * line2.startVector.x) / line1.endVector.x;
        ans[0] = line1.lineEnd;
        if (b > 999999 || line2.startVector.z * line1.endVector.x - line2.startVector.x * line1.endVector.z == 0)
            ans[1] = (line1.lineEnd + line2.lineStart) / 2;
        else
            ans[1] = new Vector3(line2.lineStart.x + b * line2.startVector.x, (line1.lineEnd.y + line2.lineStart.y) / 2, line2.lineStart.z + b * line2.startVector.z);
        ans[2] = line2.lineStart;
        return ans;
    }
    /// <summary>
    /// 路径生成算法需要的贝塞尔控制点，供linkLine调用
    /// </summary>
    private static Vector3 controlPoint(Vector3 now1, Vector3 now2, Vector3 target1, Vector3 target2)
    {
        if (target1[2] - target2[2] == 0)
        {
            return new Vector3(now2.x, 0, (now2.z + target1.z) / 2);
        }
        else if (target1[0] - target2[0] == 0)
        {
            return new Vector3((now2.x + target1.x) / 2, 0, now2.z);
        }
        else
        {
            double a1 = (now1[2] - now2[2]) / (now1[0] - now2[0]);
            double b1 = now1[2] - a1 * now1[0];
            double a2 = (target1[2] - target2[2]) / (target1[0] - target2[0]);
            double b2 = target1[2] - a2 * target1[0];
            double x = (b2 - b1) / (a1 - a2);
            double y = a1 * x + b1;
            return new Vector3((float)(x + now2[0]) / 2, 0, (float)(y + now2[2]) / 2);
        }
    }
    /// <summary>
    /// 作用同Interpolation, 算法不同
    /// </summary>
    public static Vector3[] linkLine(Line now, Line target)
    {
        Vector3[] result = new Vector3[4];
        result[0] = now.points[now.points.Length - 1];
        result[1] = controlPoint(now.points[now.points.Length - 2], now.points[now.points.Length - 1], target.points[0], target.points[1]);
        result[2] = controlPoint(target.points[1], target.points[0], now.points[now.points.Length - 1], now.points[now.points.Length - 2]);
        result[3] = target.points[0];
        return result;
    }
    /// <summary>
    /// 新的路径生成算法，适用于更普适的场景
    /// </summary>
    /// <param name="now">当前道路</param>
    /// <param name="target">目标道路</param>
    /// <returns>生成曲线</returns>
    public static Vector3[] linkLine2(Line now,Line target)
    {
        Vector3[] result = new Vector3[4];
        var len = Vector3.Distance(now.points[now.points.Length - 1], target.points[0])/3 ;
        result[0] = now.points[now.points.Length - 1];
        //计算第一个非端点贝塞尔控制点
        var a1 = now.points[now.points.Length - 2];
        var a2 = now.points[now.points.Length - 1];
        var x = len / Vector3.Distance(a1, a2)*(a2.x-a1.x);
        var z = len / Vector3.Distance(a1, a2)*(a2.z-a1.z);
        result[1].x = a2.x + x;
        result[1].z = a2.z + z;
        //计算第二个非端点贝塞尔控制点
        a1 = target.points[0];
        a2 = target.points[1];
        x = len / Vector3.Distance(a1, a2) * (a2.x - a1.x);
        z = len / Vector3.Distance(a1, a2) * (a2.z - a1.z);
        result[2].x = a1.x - x;
        result[2].z = a1.z - z;
        result[3] = target.points[0];
        return result;
    }
}
