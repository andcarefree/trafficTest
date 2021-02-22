using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    private bool isPlacingRoad = false;
    private bool isConnectingLane = false;
    private bool isSetupSource = false;

    public static List<Vector3> objectsPosition;

    public GameObject roadPrefeb;
    public GameObject carPrefeb;
    public GameObject saveDialog;

    public List<Vector3> roadPosition;
    public List<GameObject> roadList;

    void Awake()
    {
        Time.timeScale = 0;
        objectsPosition = new List<Vector3>();
    }

    void Update()
    {
        PlaceRoadObject();
        ConnectLane();
        SetupCarSource();
    }

    private void PlaceRoadObject()
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
                float scale = Vector3.Distance(roadPosition[1], roadPosition[0]) / 60.0f;

                Quaternion rotation = Quaternion.LookRotation(roadPosition[1] - roadPosition[0], Vector3.up);
                rotation *= Quaternion.Euler(0, -90f, 0);

                GameObject newRoad = Instantiate(roadPrefeb, position, rotation);
                newRoad.transform.localScale = new Vector3(scale, 1.0f, 1.0f);

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

    public void OnGenerateRoadButtonClick()
    {
        isPlacingRoad = true;
        PublicVars.current.isRoadGenerating = true;
    }

    public void OnLinkLaneButtonClick()
    {
        isConnectingLane = true;
    }

    public void OnSetCarSourceButtonClick()
    {
        isSetupSource = true;
    }

    public void OnLoadModelButtonClick()
    {
        DllReader.testInit();
    }
}
