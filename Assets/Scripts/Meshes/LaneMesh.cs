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

}
