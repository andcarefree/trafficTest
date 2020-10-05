using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    static bool playStat = false;
    static bool showState = true;

    public static bool buttonClicked = false;

    public GameObject toolPanel;
    public GameObject Road;
    public Text textref;

    public List<Vector3> roadPosition;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        Debug.Log("Awake has called");
    }

    void Update()
    {
        if (buttonClicked){
            placeObject();
        }
    }

    public void onClickStartButton()
    {
        Debug.Log("start button is clicked");
        if (!playStat)
        {
            Time.timeScale = 1;
            playStat = true;
            textref.text = "Pause";
        }
        else
        {
            Time.timeScale = 0;
            playStat = false;
            textref.text = "Start";
        }
    }
    public void onClickResetButton()
    {
        Debug.Log("stop button is pushed");
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        Time.timeScale = 0;
    }//Just for testing. need to be rewritten

    public void switchPanelShowState()
    {
        showState = !showState;
        if (showState)
        {
            toolPanel.SetActive(true);
        }
        else
        {
            toolPanel.SetActive(false);
        }
    }
    
    public void placeObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane Plane = new Plane(Vector3.up, Vector3.zero);
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (Plane.Raycast(Ray, out entry))
            {
                roadPosition.Add(Ray.GetPoint(entry));
            }
        }
        if (roadPosition.Count == 2)
        {
            Vector3 position = (roadPosition[1] + roadPosition[0]) / 2.0f;

            Quaternion rotation = Quaternion.LookRotation(roadPosition[1] - roadPosition[0], Vector3.up);
            rotation *= Quaternion.Euler(0, -90f, 0);

            Instantiate(Road, position, rotation);

            roadPosition.Clear();

            buttonClicked = false;
        }
    }

    public void onClickedButton1()
    {
        buttonClicked = true;
    }

    public enum ObjectType
    {
        Road, Source, Junction
    }
}
