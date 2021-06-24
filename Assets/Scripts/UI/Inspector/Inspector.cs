using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inspector : MonoBehaviour
{
    public static Inspector current;
    private int tableInList;
    public float RefreshInterval{ get; set; }

    [SerializeField] 
    private GameObject content;

    [SerializeField] 
    private GameObject propertyPrefab;

    [SerializeField] 
    private GameObject warningText;

    [SerializeField] 
    private GameObject scrollView;

    [field : SerializeField]
    public List<GameObject> PropertyTableList { get; set; }

    void Start()
    {
        current = this;
    }

    void Update()
    {
        // When nothing is selected, show prompt
        if (Selector.current.Selected.Count == 0)
        {
            warningText.SetActive(true);
            scrollView.SetActive(false);
        }
        else
        {
            warningText.SetActive(false);
            scrollView.SetActive(true);
        }
        
        // Check things selected
        if(Selector.current.Selected.Count != tableInList)
        {
            ShowProperty();
            tableInList = Selector.current.Selected.Count;
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
        for(int i = 0; i < PropertyTableList.Count; i++)
        {
            Destroy(PropertyTableList[i]);
            PropertyTableList.Remove(PropertyTableList[i]);
        }
        
        foreach(var gameObject in Selector.current.Selected)
        {
            if(gameObject.tag == "Car")
            {
                var properties = gameObject.GetComponent<Car>().GetType().GetFields();
                foreach(var property in properties)
                {
                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = gameObject.GetInstanceID().ToString() + ' ' + property.Name;
                    PropertyTableList.Add(propertyTable);

                    propertyTable.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.Name);
                    if(property.GetValue(gameObject.GetComponent<Car>()) != null)
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.GetValue(gameObject.GetComponent<Car>()).ToString());
                    else
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText("null"); 
                }
            }
            if(gameObject.tag == "Road")
            {
                var properties = gameObject.GetComponent<Road>().GetType().GetFields();
                foreach(var property in properties)
                {
                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = gameObject.GetInstanceID().ToString() + ' ' + property.Name;
                    PropertyTableList.Add(propertyTable);

                    propertyTable.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.Name);
                    if(property.GetValue(gameObject.GetComponent<Road>()) != null)
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText(property.GetValue(gameObject.GetComponent<Road>()).ToString());
                    else
                        propertyTable.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().SetText("null"); 
                }
            }
            if(gameObject.tag == "Lane")
            {
                var properties = gameObject.GetComponent<Line>().GetType().GetFields();
                foreach(var property in properties)
                {
                    GameObject propertyTable = Instantiate(propertyPrefab);
                    propertyTable.transform.SetParent(content.transform, false);
                    propertyTable.name = gameObject.GetInstanceID().ToString() + ' ' + property.Name;
                    PropertyTableList.Add(propertyTable);

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
        foreach (var gameObject in Selector.current.Selected)
        {
            if(gameObject.tag == "Car")
            {
                var fields = gameObject.GetComponent<Car>().GetType().GetFields();

                foreach(var field in fields)
                {
                    foreach (var propertyTable in PropertyTableList)
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
                var fields = gameObject.GetComponent<Road>().GetType().GetFields();

                foreach(var field in fields)
                {
                    foreach (var propertyTable in PropertyTableList)
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
                var fields = gameObject.GetComponent<Line>().GetType().GetFields();

                foreach(var field in fields)
                {
                    foreach (var propertyTable in PropertyTableList)
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