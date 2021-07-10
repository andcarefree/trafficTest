using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    public bool IsStop { get; set; }

    [SerializeField]
    private TextMeshProUGUI startButtonText;

    public void OnStopButtonClick()
    {
        Time.timeScale = 0;
        var cars = GameObject.FindGameObjectsWithTag("Car");
        for (int i = 0; i < cars.Length; i++)
        {
            Destroy(cars[i]);
        }

        startButtonText.SetText("开始");
    }
}
