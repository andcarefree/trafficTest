using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    [SerializeField]
    private int id;

    public void Click()
    {
        GameEvents.Instance.OnPanelSwitch(id);
    }
}
