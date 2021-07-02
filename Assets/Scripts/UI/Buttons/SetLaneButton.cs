using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetLaneButton : MonoBehaviour
{
    [SerializeField]
    private GameObject lane;

    [SerializeField]
    private Material material;

    [SerializeField]
    private TextMeshProUGUI text;

    public void OnButtonClick()
    {
        Selector.Instance.enabled = false;
        StartCoroutine(this.SetLane());
    }

    private IEnumerator SetLane()
    {
        var position = new Vector3[2];
        var state = 0;
        var newObject = (GameObject)null;

        while (true)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Destroy(newObject);
                Selector.Instance.enabled = true;

                if (newObject != null)
                {
                    Destroy(newObject);
                }
                
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

                    var pivot = (position[1] + position[0]) / 2;
                    var rotation = Quaternion.FromToRotation(Vector3.right, (position[1] - position[0]));
                    var scale = (position[1] - position[0]).magnitude / 10.0f;

                    if (newObject == null)
                    {
                        var color = material.color;
                        newObject = Instantiate(lane, pivot, rotation);

                        color.a = 0.5f;
                        material.color = color;
                        newObject.GetComponent<MeshRenderer>().material = material;
                    }

                    newObject.transform.position = pivot;
                    newObject.transform.rotation = rotation;
                    newObject.transform.localScale = new Vector3(scale, 1.0f, 1.0f);

                    if (Input.GetMouseButtonDown(0))
                    {
                        var color = material.color;

                        color.a = 1.0f;
                        material.color = color;
                        newObject.GetComponent<MeshRenderer>().material = material;
                        
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
