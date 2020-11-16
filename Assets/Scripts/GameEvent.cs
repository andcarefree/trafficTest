using System;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static GameEvent current;
    public event Action OnLoadEvent;

    void Awake()
    {
        current = this;
    }
    
    public void OnLoad()
    {
        if(OnLoadEvent != null)
        {
            OnLoadEvent();
        }
    }
}