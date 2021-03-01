using System;
using UnityEngine;

[DisallowMultipleComponent]
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public event Action OnLoadEvent;
    public event Action<int> OnDeleteEvent;

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

    public void OnDelete(int id)
    {
        if (OnDeleteEvent != null)
        {
            OnDeleteEvent(id);
        }
    }
}