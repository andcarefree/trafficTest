using UnityEngine;
using System;

public class Point : MonoBehaviour
{

    void Update()
    {
        if (transform.parent.localScale.x != 0f)
            transform.localScale = new Vector3(0.5f / transform.parent.localScale.x, 0.5f / transform.parent.localScale.y, 0.5f / transform.parent.localScale.z);
    }
}
