using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetCrossButton : MonoBehaviour
{
    [SerializeField]
    private GameObject cross;

    [SerializeField]
    private TextMeshProUGUI text;

    public void OnButtonClick()
    {
        Selector.Instance.enabled = false;
        StartCoroutine(SetCross());
    }

    private IEnumerator SetCross()
    {
        var position = new Vector3[2];
        var state = 0;

        GameObject newObject = null;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                yield break;
            }

            switch (state)
            {
                case 0:
                    if (Input.GetMouseButtonDown(0))
                    {
                        position[0] = Util.GetPointOnXZPlane(Input.mousePosition);
                        state += 1;
                    }

                    break;
                case 1:
                    position[1] = Util.GetPointOnXZPlane(Input.mousePosition);

                    if (newObject == null)
                    {
                        newObject = Instantiate(cross, position[0], Quaternion.identity);
                    }

                    var radius = (position[1] - position[0]).magnitude;
                    var scale = new Vector3(radius, 1.0f, radius);

                    newObject.transform.localScale = scale;

                    if (Input.GetMouseButtonDown(0))
                    {
                        state += 1;
                    }

                    break;
                default:
                    Selector.Instance.enabled = true;
                    yield break;
            }

            yield return null;
        }
    }
}
