using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    void Start()
    {
        if (Selector.Instance != null)
        {
            Selector.Instance.Selectable.Add(this.gameObject);
        }

        GameEvents.Instance.OnDeleteEvent += DestroySelf;
        GameEvents.Instance.OnLoadEvent += DestroySelfOnLoad;
        GameEvents.Instance.OnSelectEvent += OnSelected;
        GameEvents.Instance.OffSelectEvent += OffSelected;
    }

    private void OnDestroy()
    {
        Selector.Instance.Selectable.Remove(this.gameObject);
        PropertiyListContainer.Instance.DestroyPropertyList(this.gameObject);

        GameEvents.Instance.OnDeleteEvent -= DestroySelf;
        GameEvents.Instance.OnLoadEvent -= DestroySelfOnLoad;
        GameEvents.Instance.OnSelectEvent -= OnSelected;
        GameEvents.Instance.OffSelectEvent -= OffSelected;
    }

    private void OnSelected(int id)
    {
        if (id == gameObject.GetInstanceID())
        {
            Selector.Instance.Selected.Add(this.gameObject);

            this.gameObject.GetComponent<Outline>().enabled = true;

            PropertiyListContainer.Instance.CreatePropertyList(this.gameObject);
        }
    }

    private void OffSelected(int id)
    {
        if (id == gameObject.GetInstanceID())
        {
            Selector.Instance.Selected.Remove(this.gameObject);

            this.gameObject.GetComponent<Outline>().enabled = false;

            PropertiyListContainer.Instance.DestroyPropertyList(this.gameObject);
        }
    }
    
    private void DestroySelf(int id)
    {
        if (id == gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }

    private void DestroySelfOnLoad()
    {
        Destroy(this.gameObject);
    }
}
