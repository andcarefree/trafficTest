using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static bool playStat = false;
    static bool showState = true;
    bool button1Clicked = false;
    bool button2Clicked = false;

    public GameObject toolPanel;
    public GameObject Road;
    public static GameObject clickedRoadObject;

    public Text textref;

    public List<Vector3> roadPosition;
    public List<GameObject> roadList;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        Debug.Log("Awake has called");
    }

    void Update()
    {
        PlaceRoadObject();
        ConnectLane();
    }

    public void OnClickStartButton()
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
    public void OnClickResetButton()
    {
        // Debug.Log("stop button is pushed");
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        Time.timeScale = 0;
    }

    void SwitchPanelShowState()
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
    
    void PlaceRoadObject()
    {
        if (button1Clicked)
        {
            if (Input.GetKey(KeyCode.Escape))
            {    
                button1Clicked = false;
                roadPosition.Clear();
                return;
            }

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

                button1Clicked = false;
            }
        }
    }

    void ConnectLane()
    {
        if(button2Clicked)
        {
            if(Input.GetMouseButtonDown(0))
            {
                roadList.Add(CameraController.SelectObjectOnClick());
            }
            if(Input.GetKey(KeyCode.Escape))
            {
                roadList.Clear();
                return;
            }

            if(roadList.Count == 2)
            {
                var nextRoad = roadList[1].GetComponentsInParent<Road>();
                roadList[0].GetComponent<Line>().nextRoads = nextRoad;

                roadList.Clear();
                button2Clicked = false;
            }
        }
    }

    public void OnClickRoadButton()
    {
        button1Clicked = true;
    }

    public void OnClickLinkButton()
    {
        button2Clicked = true;
    }
}
