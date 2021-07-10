using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private TextMeshProUGUI startButtonText;

    void Awake()
    {
        Time.timeScale = 0;
        startButtonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnStartButtonClick()
    {
        if (Time.timeScale == 0f)
        {
            startButtonText.SetText("暂停");
            Time.timeScale = 1f;
        }
        else
        {
            startButtonText.SetText("开始");
            Time.timeScale = 0f;
        }
    }
}
