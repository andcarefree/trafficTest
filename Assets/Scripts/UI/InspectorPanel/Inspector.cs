using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using System;

public class Inspector : MonoBehaviour
{
    private int tableInList;
    public GameObject detectorPanel;
    public GameObject content;
    public GameObject propertyPrefab;
    public List<GameObject> propertyTableList;
    private GameObject WarningTextGO=null;
    private GameObject ScrollViewGO=null;

    public void Start()
    {
        if (detectorPanel != null)
        {
            try
            {
                WarningTextGO = detectorPanel.transform.Find("Warning Text").gameObject;
                ScrollViewGO = detectorPanel.transform.Find("Scroll View").gameObject;
            }
            catch(Exception exception)
            {
                WarningTextGO = null;
                ScrollViewGO = null;

                #if UNITY_EDITOR    
                    Debug.Log(exception);
                #endif
            }
        }
    }

    void Update()
    {
        // When nothing is selected, show prompt
        if (WarningTextGO != null && ScrollViewGO != null)
        {
            if (RectangleSelector.current.selected.Count == 0)
            {
                WarningTextGO.SetActive(true);
                ScrollViewGO.SetActive(false);
            }
            else
            {
                WarningTextGO.SetActive(false);
                ScrollViewGO.SetActive(true);
            }
        }
        // Check things selected
        if(RectangleSelector.current.selected.Count != tableInList)
        {
            ShowProperty();
            tableInList = RectangleSelector.current.selected.Count;
        }

        // Update properties of selected objects
        if(Time.timeScale != 0)
        {
            UpdateProperty();
        }
    }

    // when something is seleced, show its properties
    public void ShowProperty()
    {
        for(int i = 0; i < propertyTableList.Count; i++)
        {
            Destroy(propertyTableList[i]);
            propertyTableList.Remove(propertyTableList[i]);
        }
        
        foreach(var gameObject in RectangleSelector.current.selected)
        {
            if(gameObject.tag == "Car")
            {
                var properties = gameObject.GetComponent<Car>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach(var property in properties)
                {
                    #if UNITY_EDITOR
                        Debug.Log(property.Name);              
                    #endif

                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = gameObject.GetInstanceID().ToString() + ' ' + property.Name;
                    propertyTableList.Add(propertyTable);

                    propertyTable.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.Name);
                    if(property.GetValue(gameObject.GetComponent<Car>()) != null)
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.GetValue(gameObject.GetComponent<Car>()).ToString());
                    else
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText("null"); 
                }
            }
            if(gameObject.tag == "Road")
            {
                var properties = gameObject.GetComponent<Road>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach(var property in properties)
                {
                    #if UNITY_EDITOR
                        Debug.Log(property.Name);              
                    #endif

                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = gameObject.GetInstanceID().ToString() + ' ' + property.Name;
                    propertyTableList.Add(propertyTable);

                    propertyTable.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.Name);
                    if(property.GetValue(gameObject.GetComponent<Road>()) != null)
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.GetValue(gameObject.GetComponent<Road>()).ToString());
                    else
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText("null"); 
                }
            }
            if(gameObject.tag == "Lane")
            {
                var properties = gameObject.GetComponent<Line>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach(var property in properties)
                {
                    #if UNITY_EDITOR
                        Debug.Log(property.Name);              
                    #endif

                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = gameObject.GetInstanceID().ToString() + ' ' + property.Name;
                    propertyTableList.Add(propertyTable);

                    propertyTable.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.Name);
                    if(property.GetValue(gameObject.GetComponent<Line>()) != null)
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.GetValue(gameObject.GetComponent<Line>()).ToString());
                    else
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText("null");    
                }
            } 
        }
    }

    public void UpdateProperty()
    {
        foreach (var gameObject in RectangleSelector.current.selected)
        {
            if(gameObject.tag == "Car")
            {
                var fields = gameObject.GetComponent<Car>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                foreach(var field in fields)
                {
                    foreach (var propertyTable in propertyTableList)
                    {
                        if(propertyTable.name == gameObject.GetInstanceID().ToString() + ' ' + field.Name)
                        {
                            if(field.GetValue(gameObject.GetComponent<Car>()) != null)
                                propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(field.GetValue(gameObject.GetComponent<Car>()).ToString());
                        }
                    }
                }
            }
            if(gameObject.tag == "Road")
            {
                var fields = gameObject.GetComponent<Road>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                foreach(var field in fields)
                {
                    foreach (var propertyTable in propertyTableList)
                    {
                        if(propertyTable.name == gameObject.GetInstanceID().ToString() + ' ' + field.Name)
                        {
                            if(field.GetValue(gameObject.GetComponent<Road>()) != null)
                                propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(field.GetValue(gameObject.GetComponent<Road>()).ToString());
                        }
                    }
                }
            }
            if(gameObject.tag == "Lane")
            {
                var fields = gameObject.GetComponent<Line>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                foreach(var field in fields)
                {
                    foreach (var propertyTable in propertyTableList)
                    {
                        if(propertyTable.name == gameObject.GetInstanceID().ToString() + ' ' + field.Name)
                        {
                            if(field.GetValue(gameObject.GetComponent<Line>()) != null)
                                propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(field.GetValue(gameObject.GetComponent<Line>()).ToString());
                        }
                    }
                }
            }
        }
    }
}
