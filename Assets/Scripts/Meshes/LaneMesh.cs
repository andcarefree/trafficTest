using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class LaneMesh : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;

    void Awake()
    {
        var task = GenerateMeshAsync();
    }

    private async Task GenerateMeshAsync()
    {
        try
        {
            await Task.Yield();

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
        }
        catch(System.Exception error)
        {
            Debug.Log(error);
        }
    }

    private Vector3 QuadraicBezier(Vector3 position1, Vector3 position2, Vector3 position3, float t)
    {
        var temp1 = t * position1 + (1 - t) * position2;
        var temp2 = t * position2 + (1 - t) * position3;
        var position = t * temp1 + (1 - t) * temp2;
        return position;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        
        Gizmos.color = Color.black;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.02f);
        }
    }
}
