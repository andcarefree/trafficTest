using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public event Action OnLoadEvent;
    public event Action OnSelectEvent;

    void Awake()
    {
        current = this;
    }
    
    public void DispatchOnLoad()
    {
        if(OnLoadEvent != null)
            OnLoadEvent();
    }

    public void DispatchOnSelect()
    {
        if(OnSelectEvent != null)
            OnSelectEvent();
    }
}