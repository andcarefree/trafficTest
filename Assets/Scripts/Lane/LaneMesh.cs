using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class LaneMesh : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        normals = mesh.normals;
    }

    // 用脚本生成生成并导出mesh资源文件
    // private void GenerateMesh()
    // {
    //     mesh = new Mesh();
    //     GetComponent<MeshFilter>().mesh = mesh;

    //     vertices = new Vector3[33];

    //     for (int i = 0; i < 11; i++)
    //     {
    //         vertices[i] = new Vector3(i - 5.0f, 0f, -1.75f);    
    //     }
    //     for (int i = 11; i < 22; i++)
    //     {
    //         vertices[i] = new Vector3((float)(i - 11) - 5.0f, 0f, 0f);
    //     }
    //     for (int i = 22; i < 33; i++)
    //     {
    //         vertices[i] = new Vector3((float)(i - 22) - 5.0f, 0f, 1.75f);
    //     }

    //     mesh.vertices = vertices;

    //     triangles = new int[120];
    //     var t = 0;

    //     for (int i = 0; i < 10; i++)
    //     {
    //         triangles[t] = i;
    //         triangles[t + 1] = triangles[t + 4] = i + 11; 
    //         triangles[t + 2] = triangles[t + 3] = i + 1; 
    //         triangles[t + 5] = i + 12;

    //         t += 6;
    //     }

    //     for (int i = 11; i < 21; i++)
    //     {
    //         triangles[t] = i;
    //         triangles[t + 1] = triangles[t + 4] = i + 11; 
    //         triangles[t + 2] = triangles[t + 3] = i + 1; 
    //         triangles[t + 5] = i + 12;

    //         t += 6;
    //     }

    //     mesh.triangles = triangles;
    //     mesh.RecalculateNormals();

    //     AssetDatabase.CreateAsset(mesh, "Assets/Models/Lane.asset");
    //     AssetDatabase.SaveAssets();
    // }

    // 重新计算网格
    public void RecalculateMesh()
    {
        var childTransforms = this.GetComponentsInChildren<Transform>();
        var childPositions = new Vector3[3];
        var controlPoints = new Vector3[3];
        var offsets = new Vector3[3];
        var t = 0f;

        childPositions[0] = childTransforms[2].position;
        childPositions[1] = childTransforms[6].position;
        childPositions[2] = childTransforms[10].position; 

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
            vertices[i + 11] = QuadraicBezier(controlPoints[0], controlPoints[1], controlPoints[2], t);
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
            vertices[i] = QuadraicBezier(controlPoints[0], controlPoints[1], controlPoints[2], t);
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
            vertices[i + 22] = QuadraicBezier(controlPoints[0], controlPoints[1], controlPoints[2], t);
            t += 0.1f;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();

        normals = mesh.normals;
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
    #if UNITY_EDITOR
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
        
    #endif
    
}
