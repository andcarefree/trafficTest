using System;
using UnityEngine;

[DisallowMultipleComponent]
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public event Action OnLoadEvent;
    public event Action<int> OnDragEvent;
    public event Action<int> OnSelectedEvent;

    void Awake()
    {
        current = this;
    }
    
    public void DispatchOnLoad()
    {
        if (OnLoadEvent != null)
        {
            OnLoadEvent();
        }
    }

    public void DispatchOnDrag(int id)
    {
        if (OnDragEvent != null)
        {
            OnDragEvent(id);
        }
    }

    public void OnSelected(int id)
    {
        if (OnSelectedEvent != null)
        {
            OnSelectedEvent(id);
        }
    }
}