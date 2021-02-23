using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
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

    private IEnumerator SetUpRoad()
    {
        bool isDone = false;
        while (!isDone)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {    
                roadPosition.Clear();
                yield break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var Plane = new Plane(Vector3.up, Vector3.zero);
                var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (Plane.Raycast(Ray, out entry))
                {
                    roadPosition.Add(Ray.GetPoint(entry));
                }
            }
            if (roadPosition.Count == 2)
            {
                Vector3 position = (roadPosition[1] + roadPosition[0]) / 2.0f;
                var scale = Vector3.Distance(roadPosition[1], roadPosition[0]) / 60.0f;

                Quaternion rotation = Quaternion.LookRotation(roadPosition[1] - roadPosition[0], Vector3.up);
                rotation *= Quaternion.Euler(0, -90f, 0);

                GameObject newRoad = Instantiate(roadPrefeb, position, rotation);
                newRoad.transform.localScale = new Vector3(scale, 1.0f, 1.0f);

                roadPosition.Clear();
                isDone = true;
            }
            yield return null;
        }
    }

    private IEnumerator ConnectLane()
    {
        bool isDone = false;
        while (!isDone)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                roadList.Clear();
                yield break;
            }
            if(Input.GetMouseButtonDown(0))
            {
                roadList.Add(this.SelectObjectOnClick());
            }
            if(roadList.Count == 2)
            {
                var nextRoad = roadList[1].GetComponentsInParent<Road>();
                roadList[0].GetComponent<Line>().nextRoads = nextRoad;

                roadList.Clear();
                isDone = true;
            }
            yield return null;
        }
    }

    private IEnumerator SetUpCarSource()
    {
        bool isDone = false;
        while (!isDone)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                yield break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                var selectedRoad = this.SelectObjectOnClick();

                if (selectedRoad != null)
                {
                    OriginRoad road = selectedRoad.transform.parent.gameObject.AddComponent<OriginRoad>();
                    selectedRoad.GetComponentInParent<OriginRoad>().Car = carPrefeb;

                    isDone = true;
                }
            }       
            yield return null;
        }
    }

    private GameObject SelectObjectOnClick()
    {
        GameObject selectedObject = null;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            selectedObject = rayHit.collider.gameObject;

            #if UNITY_EDITOR
                Debug.Log(selectedObject.name);
            #endif
        }

        return selectedObject;
    }

    public void OnGenerateRoadButtonClick()
    {
        StartCoroutine(SetUpRoad());
    }

    public void OnLinkLaneButtonClick()
    {
        StartCoroutine(ConnectLane());
    }

    public void OnSetCarSourceButtonClick()
    {
        StartCoroutine(SetUpCarSource());
    }

    public void OnLoadModelButtonClick()
    {
        DllReader.testInit();
    }
}
