using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PropertyList : MonoBehaviour
{
    [SerializeField]
    private GameObject propertyPrefab;
    
    private List<GameObject> propertyNames;
    private List<GameObject> propertyValues;

    public GameObject ReferenceObject { get; set; }
    public bool IsCancelRequested { get; set; }

    void Start() 
    {
        propertyNames = new List<GameObject>();
        propertyValues = new List<GameObject>();
        IsCancelRequested = false;
        
        var objectTag = ReferenceObject.tag;

        if (objectTag == "Car")
        {
            for (int i = 0; i < 6; i++)
            {
                var property = Instantiate(propertyPrefab);
                property.transform.SetParent(transform);
                property.transform.localScale = Vector3.one;

                propertyNames.Add(property.transform.Find("Name").gameObject);
                propertyValues.Add(property.transform.Find("Value").gameObject);
            }

            propertyNames[0].GetComponent<TextMeshProUGUI>().SetText("Object ID");
            propertyValues[0].GetComponent<TextMeshProUGUI>().SetText(ReferenceObject.GetInstanceID().ToString());
            propertyNames[1].GetComponent<TextMeshProUGUI>().SetText("Object Type");
            propertyValues[1].GetComponent<TextMeshProUGUI>().SetText(ReferenceObject.tag);

            propertyNames[2].GetComponent<TextMeshProUGUI>().SetText("Velocity");
            propertyNames[3].GetComponent<TextMeshProUGUI>().SetText("Expected velocity");
            propertyNames[4].GetComponent<TextMeshProUGUI>().SetText("Accleration");
            propertyNames[5].GetComponent<TextMeshProUGUI>().SetText("Max accleration");
        }
        if (objectTag == "Lane")
        {
            var nextRoads = ReferenceObject.GetComponent<Line>().nextRoads;

            for (int i = 0; i < nextRoads.Count + 2; i++)
            {
                var property = Instantiate(propertyPrefab);
                property.transform.SetParent(transform);
                property.transform.localScale = Vector3.one;

                propertyNames.Add(property.transform.Find("Name").gameObject);
                propertyValues.Add(property.transform.Find("Value").gameObject);
            }

            propertyNames[0].GetComponent<TextMeshProUGUI>().SetText("Object ID");
            propertyValues[0].GetComponent<TextMeshProUGUI>().SetText(ReferenceObject.GetInstanceID().ToString());
            propertyNames[1].GetComponent<TextMeshProUGUI>().SetText("Object Type");
            propertyValues[1].GetComponent<TextMeshProUGUI>().SetText(ReferenceObject.tag);

            for (int i = 2; i < nextRoads.Count + 2; i++)
            {
                propertyNames[i].GetComponent<TextMeshProUGUI>().SetText($"Next Road id [{i}]");
                propertyValues[i].GetComponent<TextMeshProUGUI>().SetText(nextRoads[i - 2].gameObject.GetInstanceID().ToString());
            }
        }

        var task = UpdatePropertyValue(objectTag);
    }

    private async Task UpdatePropertyValue(string objectType)
    {
        while (!IsCancelRequested)
        {
            if (objectType == "Car")
            {
                propertyValues[2].GetComponent<TextMeshProUGUI>().SetText(ReferenceObject.GetComponent<Car>().velocity.ToString("0.00"));
                propertyValues[3].GetComponent<TextMeshProUGUI>().SetText(ReferenceObject.GetComponent<Car>().expectVelocity.ToString("0.00"));
                propertyValues[4].GetComponent<TextMeshProUGUI>().SetText(ReferenceObject.GetComponent<Car>().accel.ToString("0.00"));
                propertyValues[5].GetComponent<TextMeshProUGUI>().SetText(ReferenceObject.GetComponent<Car>().maxAccel.ToString("0.00"));
            }
            if (objectType == "Lane")
            {
                var nextRoads = ReferenceObject.GetComponent<Line>().nextRoads;

                for (int i = 2; i < nextRoads.Count + 2; i++)
                {
                    propertyValues[i].GetComponent<TextMeshProUGUI>().SetText(nextRoads[i - 2].gameObject.GetInstanceID().ToString());
                }
            }

            await Task.Delay((int)(PropertiyListContainer.Instance.RefreshInterval * 1000f));
        }
    }

    public void ChangeColorOnPointerEnter()
    {
        ReferenceObject.GetComponent<Outline>().OutlineColor = Color.green;
    }

    public void ChangeColorOnPointerExit()
    {
        ReferenceObject.GetComponent<Outline>().OutlineColor = Color.yellow;
    }
}
