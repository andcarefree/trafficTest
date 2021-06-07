using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    void Start()
    {
        if (Selector.current != null)
        {
            Selector.current.Selectable.Add(this.gameObject);
        }

        GameEvents.current.OnLoadEvent += DestorySelf;
        GameEvents.current.OnDeleteEvent += DestroySelf;
    }

    private void OnDestroy()
    {
        Selector.current.Selectable.Remove(this.gameObject);
        GameEvents.current.OnDeleteEvent -= DestroySelf;
    }
    private void DestorySelf()
    {
        Destroy(this.gameObject);
    }
    
    private void DestroySelf(int id)
    {
        if (id == gameObject.GetInstanceID())
        {
            Selector.current.Selectable.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
