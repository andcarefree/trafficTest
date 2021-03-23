using UnityEngine;
public class Util
{
    /// <summary>
    /// 修改传入浮点型变量的保留位数
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static float limit(float f)
    {
        return Mathf.Floor(f * 1000) / 1000;
    }
}