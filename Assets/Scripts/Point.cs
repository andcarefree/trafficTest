using UnityEngine;
public class Point : MonoBehaviour
{
    /// <summary>
    /// 可视化设置道路起终点所需要保留的属性
    /// </summary>
    private float scaleX;
    private float scaleY;
    private float scaleZ;
    private Vector3 mouseOffset;
    private Vector3 screenPoint;
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        // fix size 
        transform.localScale = new Vector3(0.5f / transform.parent.localScale.x, 0.5f / transform.parent.localScale.y, 0.5f / transform.parent.localScale.z);
    }
    /// <summary>
    /// 按下鼠标时触发
    /// </summary>
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        mouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
    /// <summary>
    /// 鼠标按住并拖动改变贝塞尔控制点位置
    /// </summary>
    void OnMouseDrag()
    {
        //改变贝塞尔控制点位置
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + mouseOffset;
        transform.position = currentPosition;
        GetComponentInParent<LaneMesh>().RecalculateVerticesPosition();
    }
}
