using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneMesh : MonoBehaviour
{
    Mesh mesh;
    Vector3 point0, point1, point2;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 QuadraicBezier(Vector3 position1, Vector3 position2, Vector3 position3, float t)
    {
        var temp1 = t * position1 + (1 - t) * position2;
        var temp2 = t * position2 + (1 - t) * position3;
        var position = t * temp1 + (1 - t) * temp2;
        return position;
    }
}
