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

    // 将点从屏幕坐标转换到世界坐标
    public static Vector3 GetPointOnXZPlane(Vector3 vector)
    {
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = Camera.main.ScreenPointToRay(vector);
        float entry;

        if (plane.Raycast(ray, out entry))
        {
            return ray.GetPoint(entry);
        }
        else
        {
            return Vector3.zero;
        }
    }

    // 点击返回场景中的物体
    public static GameObject SelectObjectOnClick()
    {
        GameObject selectedObject = null;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            selectedObject = rayHit.collider.gameObject;
        }

        return selectedObject;
    }
}