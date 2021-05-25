using System;
using UnityEngine;

[DisallowMultipleComponent]
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public event Action OnLoadEvent;
    public event Action<int> OnDeleteEvent;
    public event Action<Guid> OnSelectedEvent;

    void Awake()
    {
        current = this;
    }

    public void OnSelected(Guid guid)
    {
        if (OnSelectedEvent != null)
            OnSelectedEvent(guid);
    }
    
    public void DispatchOnLoad()
    {
        if (OnLoadEvent != null)
            OnLoadEvent();
    }

    public void OnDelete(int id)
    {
        if (OnDeleteEvent != null)
            OnDeleteEvent(id);
    }
}