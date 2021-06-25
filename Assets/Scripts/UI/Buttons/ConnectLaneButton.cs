using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectLaneButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statusText;

    public void OnButtonClick()
    {
        StartCoroutine(ConnectLane());
    }

    private IEnumerator ConnectLane()
    {
        var status = 0;
        var roadList = new GameObject[2];

        statusText.SetText("请点击需要被连接的前一条车道， 按ESC退出");
        while (true)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                statusText.SetText("");
                yield break;
            }

            switch (status)
            {
                case 0:
                    if (Input.GetMouseButtonDown(0))
                    {
                        roadList[0] = Selector.SelectObjectOnClick();

                        if (roadList[0] != null)
                        {
                            status += 1;
                        }
                    }
                    break;
                case 1:
                    statusText.SetText("请点击需要被连接的后一条车道， 按ESC退出");

                    if (Input.GetMouseButtonDown(0))
                    {
                        roadList[1] = Selector.SelectObjectOnClick();

                        if (roadList[1] != null)
                        {
                            status += 1;
                        }
                    }
                    break;
                default:
                    var nextRoad = roadList[1].GetComponentInParent<Road>();
                    roadList[0].GetComponent<Line>().nextRoads.Add(nextRoad);

                    statusText.SetText("连接成功");
                    yield break;
            }
            yield return null;
        }
    }
}
