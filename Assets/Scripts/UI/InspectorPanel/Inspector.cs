using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;

public class Inspector : MonoBehaviour
{
    private int tableInList;
    public GameObject detectorPanel;
    public GameObject content;
    public GameObject propertyPrefab;
    public List<GameObject> propertyTableList;
    


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

        if(RectangleSelector.current.selected.Count != tableInList)
        {
            ShowProperty();
            tableInList = RectangleSelector.current.selected.Count;
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
        
        foreach(var currentObject in RectangleSelector.current.selected)
        {
            if(currentObject.tag == "Car")
            {
                var properties = currentObject.GetComponent<Car>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach(var property in properties)
                {
                    #if UNITY_EDITOR
                        Debug.Log(property.Name);              
                    #endif

                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = property.Name;
                    propertyTableList.Add(propertyTable);

                    propertyTable.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.Name);
                    if(property.GetValue(currentObject.GetComponent<Car>()) != null)
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.GetValue(currentObject.GetComponent<Car>()).ToString());
                    else
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText("null"); 
                }
            }
            if(currentObject.tag == "Road")
            {
                var properties = currentObject.GetComponent<Road>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach(var property in properties)
                {
                    #if UNITY_EDITOR
                        Debug.Log(property.Name);              
                    #endif

                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = property.Name;
                    propertyTableList.Add(propertyTable);

                    propertyTable.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.Name);
                    if(property.GetValue(currentObject.GetComponent<Road>()) != null)
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.GetValue(currentObject.GetComponent<Road>()).ToString());
                    else
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText("null"); 
                }
            }
            if(currentObject.tag == "Lane")
            {
                var properties = currentObject.GetComponent<Line>().GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach(var property in properties)
                {
                    #if UNITY_EDITOR
                        Debug.Log(property.Name);              
                    #endif

                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = property.Name;
                    propertyTableList.Add(propertyTable);

                    propertyTable.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.Name);
                    if(property.GetValue(currentObject.GetComponent<Line>()) != null)
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.GetValue(currentObject.GetComponent<Line>()).ToString());
                    else
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText("null");    
                }
            } 
        }
    }
}
