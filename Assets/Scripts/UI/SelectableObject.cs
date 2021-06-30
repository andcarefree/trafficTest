using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private ObjectTypes objectType;

    void Start()
    {
        var tag = gameObject.tag;
        if (tag == "Car")
        {
            objectType = ObjectTypes.Car;
        }
        if (tag == "Lane")
        {
            objectType = ObjectTypes.Lane;
        }

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

    private void OnSelected()
    {
        if (objectType == ObjectTypes.Car)
        {
            
        }
        if (objectType == ObjectTypes.Lane)
        {
            
        }
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

    enum ObjectTypes
    {
        Car,
        Lane
    }
}
