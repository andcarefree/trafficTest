using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetRoadButton : MonoBehaviour
{
    [SerializeField]
    private GameObject road;
    
    [SerializeField]
    private TextMeshProUGUI text;

    public void OnButtonClick()
    {
        StartCoroutine(SetRoad());
    }

    private IEnumerator SetRoad()
    {
        GameObject parentObject = null;

        while (true)
        {
            if (Selector.current.Selected.Count == 0)
            {
                text.SetText("请选择车道，按回车键继续");

                while (true)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        break;
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        yield break;
                    }
                    yield return null;
                }
            }
            else 
            {
                parentObject = Instantiate(road);

                foreach (var gameObject in Selector.current.Selected)
                {
                    if (gameObject.tag == "Lane")
                    {
                        gameObject.transform.SetParent(parentObject.transform);
                    }
                }
                yield break;
            }
            yield return null;
        }   
    }
}
