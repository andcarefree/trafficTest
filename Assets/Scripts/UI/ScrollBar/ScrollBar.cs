using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScrollBar : MonoBehaviour
{
    public void BeingDrag()
    {
        PublicVars.current.isGUIActive = true;

        #if UNITY_EDITOR
            Debug.Log("Scroll bar is being dragged");
        #endif
    }
    
    public void EndDrag()
    {
        PublicVars.current.isGUIActive = false;

        #if UNITY_EDITOR
            Debug.Log("Scroll bar is released");
        #endif
    }
}
