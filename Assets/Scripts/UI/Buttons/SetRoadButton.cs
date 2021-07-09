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
        var parentObject = (GameObject)null;
        var parentId = 0;

        while (true)
        {
            if (Selector.Instance.Selected.Count == 0)
            {
                text.SetText("请选择车道，按回车键继续，Esc键退出");

                while (true)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (Selector.Instance.Selected.Count == 0)
                        {
                            text.SetText("未选择车道，请重新选择");
                        }
                        else
                        {
                            text.SetText("请在侧面板中选择操作，选中后按del键可以删除对象");
                            break;
                        }
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

                foreach (var gameObject in Selector.Instance.Selected)
                {
                    if (gameObject.tag == "Lane")
                    {
                        gameObject.transform.SetParent(parentObject.transform);
                        
                        text.SetText("绑定成功");
                    }
                }

                parentId = parentObject.GetInstanceID();
                GameEvents.Instance.OnRoadCreate(parentId);

                yield break;
            }
            yield return null;
        }   
    }
}
