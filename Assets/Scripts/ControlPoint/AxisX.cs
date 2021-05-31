using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisX : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 mouseOffset;

    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        mouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseDrag()
    {
        var currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        var currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + mouseOffset;
        var projectedPosition = Vector3.Project(currentPosition, Vector3.right);
        
        if (projectedPosition != Vector3.zero)
        {
            var parentPosition = transform.parent.position;
            transform.parent.position = new Vector3(projectedPosition.x, parentPosition.y, parentPosition.z); 
        }
        
        GetComponentInParent<Lane>().RecalculateMesh();
    }
}
