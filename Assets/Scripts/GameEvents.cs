using System;
using UnityEngine;

[DisallowMultipleComponent]
public class GameEvents : MonoBehaviour
{
    private static GameEvents _instance;
    public static GameEvents Instance { get => _instance; }
    public event Action OnLoadEvent;
    public event Action<int> OnRoadCreateEvent;
    public event Action<int> OnDeleteEvent;
    public event Action<int> PanelSwitchEvent;
    public event Action<int> OnSelectEvent;
    public event Action<int> OffSelectEvent;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        if (_instance == null)
        {
            _instance= this;
        }
    }

    public void OnLoad()
    {
        if (OnLoadEvent != null)
            OnLoadEvent();
    }

    public void OnDelete(int id)
    {
        if (OnDeleteEvent != null)
            OnDeleteEvent(id);
    }

    public void OnRoadCreate(int id)
    {
        if (OnRoadCreateEvent != null)
            OnRoadCreateEvent(id);
    }

    public void OnSelect(int id)
    {
        if (OnSelectEvent != null)
            OnSelectEvent(id);
    }

    public void OffSelect(int id)
    {
        if (OffSelectEvent != null)
            OffSelectEvent(id);
    }

    public void OnPanelSwitch(int id)
    {
        PanelSwitchEvent(id);
    }
}