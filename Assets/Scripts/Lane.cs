using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class Lane : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private int[] triangles;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        normals = mesh.normals;
        triangles = mesh.triangles;
    }

    void Update()
    {
        // RecalculateMesh();

        for (int i = 0; i < triangles.Length / 3; i += 3)
        {
            Debug.Log($"{triangles[i]},{triangles[i + 1]},{triangles[i + 2]}");
        }
    }

    // 重新计算网格
    public void RecalculateMesh()
    {
        var childTransforms = this.GetComponentsInChildren<Transform>();
        var childPositions = new Vector3[3];
        var controlPoints = new Vector3[3];
        var offsets = new Vector3[3];
        var t = 0f;

        for (int i = 0; i < 3; i++)
        {
            childPositions[i] = childTransforms[i + 1].position;
            Debug.Log($"{childTransforms[i + 1].name}:{childPositions[i]}"); 
        }

        offsets[0] = Quaternion.Euler(0, 90, 0) * (childPositions[1] - childPositions[0]).normalized;
        offsets[1] = Quaternion.Euler(0, 90, 0) * (childPositions[2] - childPositions[0]).normalized;
        offsets[2] = Quaternion.Euler(0, 90, 0) * (childPositions[2] - childPositions[1]).normalized;

        // 计算中轴线的顶点位置
        for (int i = 0; i < 3; i++)
        {
            controlPoints[i] = childPositions[i];
        }
        
        for (int i = 0; i < 11; i++)
        {
            vertices[3 * i + 1] = QuadraicBezier(controlPoints[0], controlPoints[1], controlPoints[2], t);
            t += 0.1f;
        }

        // 计算右侧(车辆前进方向)的顶点位置
        for (int i = 0; i < 3; i++)
        {
            controlPoints[i] = childPositions[i] + 1.75f * offsets[i];
        }

        t = 0f;

        for (int i = 0; i < 11; i++)
        {
            vertices[3 * i] = QuadraicBezier(controlPoints[0], controlPoints[1], controlPoints[2], t);
            t += 0.1f;
        }
        
        // 计算左侧的顶点位置
        for (int i = 0; i < 3; i++)
        {
            controlPoints[i] = childPositions[i] - 1.75f * offsets[i];
        }
        
        t = 0f;

        for (int i = 0; i < 11; i++)
        {
            vertices[3 * i + 2] = QuadraicBezier(controlPoints[0], controlPoints[1], controlPoints[2], t);
            t += 0.1f;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    // 贝塞尔函数，用于计算顶点位置
    private Vector3 QuadraicBezier(Vector3 position1, Vector3 position2, Vector3 position3, float t)
    {
        var temp1 = (1 - t) * position1 + t * position2;
        var temp2 = (1 - t) * position2 + t * position3;
        var position = (1 - t) * temp1 + t * temp2;
        return position;
    }

    // 调试用
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.02f);
            UnityEditor.Handles.Label(vertices[i], $"{i}");

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }
}
