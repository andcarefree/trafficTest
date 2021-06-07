﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLaneButton : MonoBehaviour
{
    [SerializeField]
    private GameObject lane;

    [SerializeField]
    private Material material;

    public void OnButtonClick()
    {
        StartCoroutine(this.SetLane());
    }

    private IEnumerator SetLane()
    {
        var position = new Vector3[2];
        var state = 0;

        GameObject newObject = null;

        while (true)
        {
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
                    var scale = (position[1] - position[0]).magnitude / 200f;

                    if (newObject == null)
                    {
                        var color = material.color;
                        newObject = Instantiate(lane, pivot, rotation);

                        color.a = 0.5f;
                        material.color = color;
                        newObject.GetComponent<LineRenderer>().material = material;
                    }

                    newObject.transform.position = pivot;
                    newObject.transform.rotation = rotation;
                    newObject.transform.localScale = new Vector3(scale, 1.0f, 1.0f);

                    if (Input.GetMouseButtonDown(0))
                    {
                        var color = material.color;

                        color.a = 1.0f;
                        material.color = color;
                        newObject.GetComponent<LineRenderer>().material = material;
                        
                        state += 1;
                    }

                    break;

                default:
                    
                    yield break;

            }

            yield return null;
        }
    }
}
