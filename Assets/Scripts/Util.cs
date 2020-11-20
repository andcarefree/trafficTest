using UnityEngine;
using UnityEditor;

public class Util
{
    /// <summary>
    /// 浮点数的小数点位数限制
    /// </summary>
    public static float limit(float f)
    {
        return Mathf.Floor(f * 1000) / 1000;
    }
}