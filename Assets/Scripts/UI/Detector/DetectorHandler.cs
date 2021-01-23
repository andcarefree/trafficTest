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
        if(RectangleSelector.current.selected.Count == 0)
        {
            detectorPanel.transform.Find("WarningText").gameObject.SetActive(true);
        }
        else
        {
            detectorPanel.transform.Find("WarningText").gameObject.SetActive(false);
        }
    }
}
