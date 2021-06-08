using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetSourceButton : MonoBehaviour
{
    [SerializeField]
    public GameObject car;

    [SerializeField]
    private TextMeshProUGUI text;

    public void OnButtonClick()
    {
        StartCoroutine(SetCarSource());
    }

    private IEnumerator SetCarSource()
    {
        bool isDone = false;

        text.SetText("请点击需要设置为车辆源的道路，点击车道即可");
        while (!isDone)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                text.SetText("");
                yield break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                var selectedObject = Selector.SelectObjectOnClick();

                if (selectedObject.tag == "Lane")
                {
                    OriginRoad originRoad = selectedObject.transform.parent.gameObject.AddComponent<OriginRoad>();
                    originRoad.Car = car;
                    selectedObject.GetComponentInParent<Road>().roadType = RoadTypeEnum.SOURCE;

                    isDone = true;
                }
                else
                {
                    selectedObject = null;
                    text.SetText("所点击的不是道路，请重新选择");
                }
            }
            yield return null;
        }

        text.SetText("");
    }
}
