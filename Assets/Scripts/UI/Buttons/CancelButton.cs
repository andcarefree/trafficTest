using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour
{
    public void OnCancelButtonClick()
    {
        transform.parent.parent.gameObject.SetActive(false);
    }
}
