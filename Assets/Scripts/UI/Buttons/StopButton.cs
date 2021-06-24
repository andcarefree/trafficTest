using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    public void OnStopButtonClick()
    {
        Time.timeScale = 0;
        var cars = GameObject.FindGameObjectsWithTag("Car");
        for (int i = 0; i < cars.Length; i++)
        {
            Destroy(cars[i]);
        }
    }
}
