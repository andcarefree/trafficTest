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
        text.SetText("请点击需要设置为车辆源的道路，点击车道即可");
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                text.SetText("");
                yield break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                var selectedObject = Selector.SelectObjectOnClick();

                if (selectedObject != null && selectedObject.tag == "Lane")
                {
                    selectedObject.GetComponentInParent<OriginRoad>().enabled = true;
                    selectedObject.GetComponentInParent<Road>().roadType = RoadTypes.SOURCE;

                    break;
                }
                else
                {
                    text.SetText("所点击的不是道路，请重新选择");
                }
            }
            yield return null;
        }

        text.SetText("");
    }
}
