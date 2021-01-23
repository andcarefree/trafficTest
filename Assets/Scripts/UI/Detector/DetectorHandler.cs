using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorHandler : MonoBehaviour
{
    public static DetectorHandler detector;
    public GameObject detectorPanel;

    void Start()
    {
        
    }

    void Update()
    {
        // When nothing is selected, show prompt
        if(RectangleSelector.current.selected.Count == 0)
        {
            detectorPanel.transform.Find("WarningText").gameObject.SetActive(true);
        }
        else
        {
            detectorPanel.transform.Find("WarningText").gameObject.SetActive(false);
        }

        // When a single thing is selected, show its properties
        if(RectangleSelector.current.selected.Count == 1)
        {
            var currentObject = RectangleSelector.current.selected[0];
            if(currentObject.tag == "Car")
            {
                var properties = Type.GetType("Car").GetProperties();
                for(int i = 0; i < properties.Length; i++)
                { 
                    Debug.Log(properties[i].ToString());
                }            
            }
        }
    }
}
