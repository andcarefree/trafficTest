using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DetectorHandler : MonoBehaviour
{
    public static DetectorHandler detector;
    public GameObject detectorPanel;
    public GameObject propertyPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        // When nothing is selected, show prompt
        if(RectangleSelector.current.selected.Count == 0)
        {
            detectorPanel.transform.Find("Warning Text").gameObject.SetActive(true);
            detectorPanel.transform.Find("Scroll View").gameObject.SetActive(false);

        }
        else
        {
            detectorPanel.transform.Find("Warning Text").gameObject.SetActive(false);
            detectorPanel.transform.Find("Scroll View").gameObject.SetActive(true);

        }

        // When a single thing is selected, show its properties
        if(RectangleSelector.current.selected.Count == 1)
        {
            var currentObject = RectangleSelector.current.selected[0];
            if(currentObject.tag == "Car")
            {
                var properties = currentObject.GetComponent<Car>().GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach(var property in properties)
                { 
                    Debug.Log(property.Name);
                    // GameObject gameObject = Instantiate(propertyPrefab);
                    // gameObject.transform.SetParent(detectorPanel.transform.Find("Content"), false);
                }   
            }         
        }
    }
}
