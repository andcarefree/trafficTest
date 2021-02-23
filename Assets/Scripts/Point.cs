using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private float scaleX, scaleY, scaleZ;

    void Update()
    {
        // fix size 
        transform.localScale = new Vector3(0.5f / transform.parent.localScale.x, 0.5f / transform.parent.localScale.y, 0.5f / transform.parent.localScale.z);
    }
    void OnMouseDrag()
    {
        // Change Position
        Vector3 selectPosition= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectPosition.y = 0;
        transform.position = selectPosition;
    }
}
