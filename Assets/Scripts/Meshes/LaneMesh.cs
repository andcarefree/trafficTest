using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class LaneMesh : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;

    void Awake()
    {
        GenerateMeshAsync();
    }

    private void GenerateMeshAsync()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Lane";
        
        // Generate vertices of the road, 44 vertices in total
        int v = 0;
        vertices = new Vector3[44];

        for (float y = 0; y <= 0.1f; y += 0.1f)
        {
            for (int x = 0; x <= 10; x++)
            {
                vertices[v++] = new Vector3(x, y, 0);
            }
            for (int x = 0; x <= 10; x++)
            {
                vertices[v++] = new Vector3(10 - x, y, 3.5f);
            }
        }

        mesh.vertices = vertices;

        // Generate triangles, 84 triangles, 252 vertices of triangles in total
        int[] triangles = new int[252];
        int t = 0;

        // side faces
        for (int i = 0; i < 22; i++)
        {
            triangles[t] = i % 22;
            triangles[t + 1] = triangles[t + 4] = (22 + i) % 22 + 22;
            triangles[t + 2] = triangles[t + 3] = (i + 1) % 22;
            triangles[t + 5] = (22 + i + 1) % 22 + 22;

            t += 6;
        }

        // bottom faces
        for (int i = 0; i < 10; i++)
        {
            triangles[t] = 21 - i;
            triangles[t + 1] = triangles[t + 4] = i;
            triangles[t + 2] = triangles[t + 3] = 20 - i;
            triangles[t + 5] = 1 + i; 

            t += 6;
        }

        // top faces
        for (int i = 0; i < 10; i++)
        {
            triangles[t] = 22 + i;
            triangles[t + 1] = triangles[t + 4] = 43 - i;
            triangles[t + 2] = triangles[t + 3] = 23 + i;
            triangles[t + 5] = 42 - i; 

            t += 6;
        } 
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        normals = mesh.normals;

        gameObject.AddComponent<MeshCollider>();
        gameObject.AddComponent<Outline>();
    }

    public void RecalculateVerticesPosition()
    {
        var childTransforms = this.GetComponentsInChildren<Transform>();
        var childPositions = new Vector3[3];
        var controlPoints = new Vector3[3];
        var offsets = new Vector3[3];
        var t = 0f;
        
        for (int i = 0; i < 3; i++)
        {
            childPositions[i] = childTransforms[i + 1].position;
        }

        offsets[0] = Quaternion.Euler(0, 90, 0) * (childPositions[1] - childPositions[0]).normalized;
        offsets[1] = Quaternion.Euler(0, 90, 0) * (childPositions[2] - childPositions[0]).normalized;
        offsets[2] = Quaternion.Euler(0, 90, 0) * (childPositions[2] - childPositions[1]).normalized;

        // calculate vertices from 2 edges of the front face
        for (int i = 0; i < 3; i++)
        {
            controlPoints[i] = childPositions[i] + 1.75f * offsets[i];
        }

        for (int i = 0; i < 11; i++)
        {
            vertices[i] = QuadraicBezier(controlPoints[0], controlPoints[1], controlPoints[2], t);
            vertices[22 + i] = vertices[i] + new Vector3(0, 0.1f, 0);
            t += 0.1f;
        }
        
        // calculate vertices from 2 edges of the back face
        for (int i = 0; i < 3; i++)
        {
            controlPoints[i] = childPositions[i] - 1.75f * offsets[i];
        }
        t = 0f;

        for (int i = 0; i < 11; i++)
        {
            vertices[21 - i] = QuadraicBezier(controlPoints[0], controlPoints[1], controlPoints[2], t);
            vertices[43 - i] = vertices[21 - i] + new Vector3(0, 0.1f, 0);
            t += 0.1f;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();

        normals = mesh.normals;
    }

    private Vector3 QuadraicBezier(Vector3 position1, Vector3 position2, Vector3 position3, float t)
    {
        var temp1 = (1 - t) * position1 + t * position2;
        var temp2 = (1 - t) * position2 + t * position3;
        var position = (1 - t) * temp1 + t * temp2;
        return position;
    }

    // for debug
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }
}
