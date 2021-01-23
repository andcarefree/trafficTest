using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    private GameObject clickedObject;
    private GameObject cached;

    void Update()
    {
        cached = ButtonHandler.SelectObjectOnClick();
        if(cached != null)
        {
            clickedObject = cached;
        }

    }

    public void OnClick()
    {
        Debug.Log(clickedObject);
        if(clickedObject.tag == "Car")
        {
            clickedObject.GetComponent<Car>().stopTest = true;
        }
    }
}

