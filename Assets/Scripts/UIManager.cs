using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static bool playStat = false;
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        Debug.Log("Awake has called");
    }
    void Start()
    {
        Debug.Log("start has called");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClickStartButton()
    {
        Debug.Log("start button is clicked");
        if (!playStat)
        {
            Time.timeScale = 1;
            playStat = true;
        }
        else
        {
            Time.timeScale = 0;
            playStat = false;
        }
    }
    public void changeButtonText(Text textref)
    {
        if (playStat)
            textref.text = "Pause";
        else
            textref.text = "Start";
    }
    public void onClickResetButton()
    {
        Debug.Log("stop button is pushed");
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        Time.timeScale = 0;
    }
}
