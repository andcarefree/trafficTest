using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public event Action OnLoadEvent;

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
}