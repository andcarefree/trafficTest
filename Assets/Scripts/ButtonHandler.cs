using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class ButtonHandler : MonoBehaviour
{
    public static bool playStat = false;
    bool showState = true;
    bool button1Clicked = false;
    bool button2Clicked = false;
    bool button3Clicked = false;

    public static List<Vector3> objectsPosition;
    public static List<GameObject> objectsInRect;

    public GameObject roadPrefeb;
    public GameObject carPrefeb;

    public TextMeshPro textref;

    public List<Vector3> roadPosition;
    public List<GameObject> roadList;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        objectsPosition = new List<Vector3>();
    }

    void Update()
    {
        SelectObjectOnClick();
        PlaceRoadObject();
        ConnectLane();
        SetupCarSource();
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

                Instantiate(roadPrefeb, position, rotation);

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
                roadList.Add(ButtonHandler.SelectObjectOnClick());
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

    void SetupCarSource()
    {
        if (button3Clicked)
        {
            if(Input.GetMouseButtonDown(0))
            {
                var selectedRoad = ButtonHandler.SelectObjectOnClick();

                OriginRoad road = selectedRoad.transform.parent.gameObject.AddComponent<OriginRoad>();
                selectedRoad.GetComponentInParent<OriginRoad>().Car = carPrefeb;

                button3Clicked = false;
            }       
        }
    }

    public static void SelectObjectInRect()
    {

    }

    public static GameObject SelectObjectOnClick()
    {
        GameObject selectedObject = null;

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit))
            {
                selectedObject = rayHit.collider.gameObject;
                Debug.Log(selectedObject.name);
            }
        }
        return selectedObject;
    }

    public void OnClickStartButton()
    {
        // Debug.Log("start button is clicked");
        if (!playStat)
        {
            Time.timeScale = 1;
            playStat = true;
            textref.text = "暂停";
        }
        else
        {
            Time.timeScale = 0;
            playStat = false;
            textref.text = "开始";
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

    public void OnClickOriginButton()
    {
        button3Clicked = true;
    }

    public void OnApplicationQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
