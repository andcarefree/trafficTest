using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    public static bool playStat = false;
    private static bool isPlacingRoad = false;
    private static bool isConnectingLane = false;
    private static bool isSetupSource = false;

    public static List<Vector3> objectsPosition;
    public static List<GameObject> objectsInRect;

    public GameObject roadPrefeb;
    public GameObject carPrefeb;
    public GameObject saveDialog;

    public TextMeshProUGUI textref;

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
        if (isPlacingRoad)
        {
            if (Input.GetKey(KeyCode.Escape))
            {    
                isPlacingRoad = false;
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

                isPlacingRoad = false;
            }
        }
    }

    void ConnectLane()
    {
        if(isConnectingLane)
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
                isConnectingLane = false;
            }
        }
    }

    void SetupCarSource()
    {
        if (isSetupSource)
        {
            if(Input.GetMouseButtonDown(0))
            {
                var selectedRoad = ButtonHandler.SelectObjectOnClick();

                OriginRoad road = selectedRoad.transform.parent.gameObject.AddComponent<OriginRoad>();
                selectedRoad.GetComponentInParent<OriginRoad>().Car = carPrefeb;

                isSetupSource = false;
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
        isPlacingRoad = true;
    }

    public void OnClickLinkButton()
    {
        isConnectingLane = true;
    }

    public void OnClickOriginButton()
    {
        isSetupSource = true;
    }

    public void OnApplicationQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnSave()
    {
        saveDialog.SetActive(true);
    }

    public void ConfirmSave()
    {

        // SerializationManager.Save(, SaveData.current);
    }

    public void CancelSave()
    {
        saveDialog.SetActive(false);
    }
}
