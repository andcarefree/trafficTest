using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private float scaleX, scaleY, scaleZ;
    private Vector3 mouseOffset;
    private Vector3 screenPoint;

    void Update()
    {
        // fix size 
        transform.localScale = new Vector3(0.5f / transform.parent.localScale.x, 0.5f / transform.parent.localScale.y, 0.5f / transform.parent.localScale.z);
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        mouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        // Change Position
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + mouseOffset;
        transform.position = currentPosition;
    }
}
