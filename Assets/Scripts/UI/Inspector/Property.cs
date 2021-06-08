using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Property : MonoBehaviour
{
    // 用来换掉旧的探测器脚本，用反射慢的离谱 
    
    public static Property current;
    [SerializeField] private TextMeshProUGUI[] propertyNames;
    [SerializeField] private TextMeshProUGUI[] propertyValue;

    private void Start() 
    {
        current = this;
    }

    private IEnumerator GetPropertyValue(GameObject gameObject)
    {
        if (gameObject.tag == "Car")
        {
            // gameObject.GetComponent<Car>().accel
        }
        if (gameObject.tag == "Lane")
        {
            
        }
        if (gameObject.tag == "Road")
        {
            
        }

        yield return new WaitForSeconds(GetComponentInParent<Inspector>().RefreshInterval);
    }
}
