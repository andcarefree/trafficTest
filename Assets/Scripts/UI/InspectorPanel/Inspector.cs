using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 探测器面板脚本，目前使用反射实现
// 在未来确定了路，车，路口的模式之后会改用更好的方法实现
public class Inspector : MonoBehaviour
{
    public static Inspector current;
    private int tableInList;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject propertyPrefab;
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject scrollView;
    private List<GameObject> propertyTableList = new List<GameObject>();
    public List<GameObject> PropertyTableList { get => propertyTableList; set => propertyTableList = value; }

    void Start()
    {
        current = this;
    }

    void Update()
    {
        // 当没有任何物件被选中的时候，显示提示
        if (RectangleSelector.current.Selected.Count == 0)
        {
            warningText.SetActive(true);
            scrollView.SetActive(false);
        }
        else
        {
            warningText.SetActive(false);
            scrollView.SetActive(true);
        }
        
        // 使用数量确认是否有东西选中
        if(RectangleSelector.current.Selected.Count != tableInList)
        {
            ShowProperty();
            tableInList = RectangleSelector.current.Selected.Count;
        }

        // 在探测器中更新选中物件的属性
        if(Time.timeScale != 0)
        {
            UpdateProperty();
        }
    }

    // 当任何物件被选中时，在探测器面板展示其内部属性
    public void ShowProperty()
    {
        for(int i = 0; i < propertyTableList.Count; i++)
        {
            Destroy(propertyTableList[i]);
            propertyTableList.Remove(propertyTableList[i]);
        }
        
        foreach(var gameObject in RectangleSelector.current.Selected)
        {
            if(gameObject.tag == "Car")
            {
                var properties = gameObject.GetComponent<Car>().GetType().GetFields();
                foreach(var property in properties)
                {
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
                var properties = gameObject.GetComponent<Road>().GetType().GetFields();
                foreach(var property in properties)
                {
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
                var properties = gameObject.GetComponent<Line>().GetType().GetFields();
                foreach(var property in properties)
                {
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

    // 更新已经显示的属性
    public void UpdateProperty()
    {
        foreach (var gameObject in RectangleSelector.current.Selected)
        {
            if(gameObject.tag == "Car")
            {
                var fields = gameObject.GetComponent<Car>().GetType().GetFields();

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
                var fields = gameObject.GetComponent<Road>().GetType().GetFields();

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
                var fields = gameObject.GetComponent<Line>().GetType().GetFields();

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
