using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI.Panel.FileSelet;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusBar;
    [SerializeField] private GameObject roadPrefeb;
    [SerializeField] private GameObject carPrefeb;
    [SerializeField] private FileSelectPanel fileSelectPanel;
    private GameObject[] roadList;
    private Vector3[] roadPosition;


    void Awake()
    {
        Time.timeScale = 0;
    }

    void Start()
    {
        StartCoroutine(DestroyObjectOnLick());
        Debug.Log(Application.persistentDataPath);
    }

    private IEnumerator SetUpRoad()
    {
        int status = 1;
        var roadPosition = new Vector3[2];

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
        var roadList = new GameObject[2];

        statusBar.SetText("请点击需要被连接的前一条车道， 按ESC退出");
        while (status == 1)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                statusBar.SetText("");
                yield break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                roadList[0] = this.SelectObjectOnClick();

                if (roadList[0] != null)
                {
                    status += 1;
                }
            }
            yield return null;
        }

        statusBar.SetText("请点击需要被连接的后一条车道， 按ESC退出");
        while (status == 2)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                statusBar.SetText("");
                yield break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                roadList[1] = this.SelectObjectOnClick();

                if (roadList[1] != null)
                {
                    status += 1;
                }
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
                    OriginRoad originRoad = selectedRoad.transform.parent.gameObject.AddComponent<OriginRoad>();
                    originRoad.Car = carPrefeb;
                    selectedRoad.GetComponentInParent<Road>().roadType = RoadTypeEnum.SOURCE;

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

    private IEnumerator DestroyObjectOnLick()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                var deleteList = RectangleSelector.current.Selected;
                var propertyTable = Inspector.current.PropertyTableList;
                
                for (int i = 0; i < deleteList.Count; i++)
                {
                    GameEvents.current.OnDelete(deleteList[i].GetInstanceID());
                }

                for (int i = 0; i < propertyTable.Count; i++)
                {
                    Destroy(propertyTable[i]);
                }
                RectangleSelector.current.Selected.Clear();
            }
            yield return null;
        }
    }

    public void OnStopButtonClick()
    {
        Time.timeScale = 0;
        var cars = GameObject.FindGameObjectsWithTag("Car");
        for (int i = 0; i < cars.Length; i++)
        {
            Destroy(cars[i]);
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
        //DllReader.testInit();
        if (fileSelectPanel != null)
            fileSelectPanel.Activate(DllSelect);
    }

    private void DllSelect()
    {
        DllReader.LoadDll(UI.Panel.FileSelet.FileSelectPanel.currentFileSelectPanel.filePath);
    }


}
