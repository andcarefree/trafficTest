using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 继承IBeginDragHandler, IEndDragHandler接口，保证在滚动条拖拽时不会触发其他操作
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
