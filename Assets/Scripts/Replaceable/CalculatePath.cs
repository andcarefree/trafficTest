using UnityEngine;


public class CalculatePath
{
    public delegate Vector3[] ICalculatePath(Vector3 position, Vector3 foward, Vector3 linePosition, Vector3 targetLinePosition);

    /// <summary>
    /// 换道路径生成算法
    /// ///TODO 该算法应该是可供二次开发的
    /// </summary>
    public Vector3[] MyCalculatePath(Vector3 position, Vector3 foward, Vector3 linePosition ,Vector3 targetLinePosition)
    {
        Vector3[] res = new Vector3[4];
        res[0] = position;
        res[1] = position + 7 * foward;
        res[2] = res[1] + (targetLinePosition - linePosition);
        res[3] = res[2] + 7 * foward;
        return res;
    }

    public Vector3[] ChangePath(Vector3 position ,Vector3 foward,Vector3 linePosition, Vector3 targetLinePosition,ICalculatePath method)
    {
        return method(position, foward,linePosition, targetLinePosition);
    }
}