using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPanel : MonoBehaviour
{
    [SerializeField]
    private int id;

    void Start()
    {
        GameEvents.current.PanelSwitchEvent += ShowPanelOnButtonClick;
    }

    void OnDestroy()
    {
        GameEvents.current.PanelSwitchEvent -= ShowPanelOnButtonClick;
    }

    public void ShowPanelOnButtonClick(int id)
    {
        if (id == this.id)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

