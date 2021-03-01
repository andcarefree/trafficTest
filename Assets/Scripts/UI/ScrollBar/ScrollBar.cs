using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollBar : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public static bool isGUIActive = false;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        isGUIActive = true;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        isGUIActive = false;
    }
}
