using System;
using UnityEngine;

[DisallowMultipleComponent]
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public event Action OnLoadEvent;
    public event Action<int> OnDeleteEvent;
    public event Action<int> PanelSwitchEvent;
    public event Action<Guid> OnSelectedEvent;

    void Awake()
    {
        current = this;
    }

    public void OnDelete(int id)
    {
        if (OnDeleteEvent != null)
            OnDeleteEvent(id);
    }

    public void OnPanelSwitch(int id)
    {
        PanelSwitchEvent(id);
    }
}