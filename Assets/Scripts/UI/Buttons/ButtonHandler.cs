using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusBar;
    [SerializeField] private GameObject roadPrefeb;
    [SerializeField] private GameObject carPrefeb;
    private GameObject[] roadList = new GameObject[2];
    private Vector3[] roadPosition = new Vector3[2];

    void Awake()
    {
        Time.timeScale = 0;
    }

    void Start()
    {
        StartCoroutine(destroyObjectOnLick());
    }

    private IEnumerator SetUpRoad()
    {
        int status = 1;

        statusBar.SetText("请点击道路的第一个点（起点），按ESC键退出");
        while (status == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {    
                statusBar.SetText("");
                yield break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var Plane = new Plane(Vector3.up, Vector3.zero);
                var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (Plane.Raycast(Ray, out entry))
                {
                    roadPosition[0] = Ray.GetPoint(entry);
                }
                status += 1;
            }
            yield return null;
        }

        statusBar.SetText("请点击道路的第二个点（终点），按ESC键退出");
        while (status == 2)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {    
                statusBar.SetText("");
                yield break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var Plane = new Plane(Vector3.up, Vector3.zero);
                var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (Plane.Raycast(Ray, out entry))
                {
                    roadPosition[1] = Ray.GetPoint(entry);
                }
                status += 1;
            }
            yield return null;
        }

        Vector3 position = (roadPosition[1] + roadPosition[0]) / 2.0f;
        var scale = Vector3.Distance(roadPosition[1], roadPosition[0]) / 60.0f;

        Quaternion rotation = Quaternion.LookRotation(roadPosition[1] - roadPosition[0], Vector3.up);
        rotation *= Quaternion.Euler(0, -90f, 0);

        GameObject newRoad = Instantiate(roadPrefeb, position, rotation);
        newRoad.transform.localScale = new Vector3(scale, 1.0f, 1.0f);

        statusBar.SetText("");
    }

    private IEnumerator ConnectLane()
    {
        int status = 1;

        statusBar.SetText("请点击需要被连接的前一条车道， 按ESC退出");
        while (status == 1)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                statusBar.SetText("");
                yield break;
            }
            if(Input.GetMouseButtonDown(0))
            {
                roadList[1] = this.SelectObjectOnClick();
                status += 1;
            }
            yield return null;
        }

        statusBar.SetText("请点击需要被连接的后一条车道， 按ESC退出");
        while (status == 2)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                statusBar.SetText("");
                yield break;
            }
            if(Input.GetMouseButtonDown(0))
            {
                roadList[1] = this.SelectObjectOnClick();
                status += 1;
            }
            yield return null;
        }

        var nextRoad = roadList[1].GetComponentsInParent<Road>();
        roadList[0].GetComponent<Line>().nextRoads = nextRoad;

        statusBar.SetText("");
    }

    private IEnumerator SetUpCarSource()
    {
        bool isDone = false;

        statusBar.SetText("请点击需要设置为车辆源的道路");
        while (!isDone)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                statusBar.SetText("");
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

        statusBar.SetText("");
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

    private IEnumerator destroyObjectOnLick()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                var deleteList = RectangleSelector.current.Selected;
                for (int i = 0; i < deleteList.Count; i++)
                {
                    GameEvents.current.OnDelete(deleteList[i].GetInstanceID());
                }
            }
            yield return null;
        }
    }

    public void OnGenerateRoadButtonClick()
    {
        if (Time.timeScale == 0)
        {
            StartCoroutine(SetUpRoad());
        }
        else
        {
            statusBar.SetText("正在播放中，请先停止再进行编辑");
        }
    }

    public void OnLinkLaneButtonClick()
    {
        if (Time.timeScale == 0)
        {
            StartCoroutine(ConnectLane());
        }
        else
        {
            statusBar.SetText("正在播放中，请先停止再进行编辑");
        }
    }

    public void OnSetCarSourceButtonClick()
    {
        if (Time.timeScale == 0)
        {
            StartCoroutine(SetUpCarSource());
        }
        else
        {
            statusBar.SetText("正在播放中，请先停止再进行编辑");
        }
    }

    public void OnLoadModelButtonClick()
    {
        DllReader.testInit();
    }
}
