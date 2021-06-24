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
        int status = 1;
        var roadList = new GameObject[2];

        statusText.SetText("请点击需要被连接的前一条车道， 按ESC退出");
        while (status == 1)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                statusText.SetText("");
                yield break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                roadList[0] = Util.SelectObjectOnClick();

                if (roadList[0] != null)
                {
                    status += 1;
                }
            }
            yield return null;
        }

        statusText.SetText("请点击需要被连接的后一条车道， 按ESC退出");
        while (status == 2)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                statusText.SetText("");
                yield break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                roadList[1] = Util.SelectObjectOnClick();

                if (roadList[1] != null)
                {
                    status += 1;
                }
            }
            yield return null;
        }

        var nextRoad = roadList[1].GetComponentInParent<Road>();
        roadList[0].GetComponent<Line>().nextRoads.Add(nextRoad);

        statusText.SetText("");
    }
}
