using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCamera : MonoBehaviour
{
    private Camera m_camera;

    public float fps;
    //public float axis;
    
    private void Start()
    {
        m_camera = GetComponent<Camera>();
    }
    private void Update()
    {
        m_camera.orthographicSize -= 20*Input.GetAxis("Mouse ScrollWheel");
        transform.position+= (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")))*5;
        fps = 1.0f/Time.deltaTime;
    }
}
