using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float moveSpeed;
    public float moveTime;
    public float rotationSpeed;
    public float zoomAmount;

    private Vector3 newPosition;
    private Vector3 dragStartPosition;
    private Vector3 dragStopPosition;
    private Vector3 rotationStartPosition;
    private Vector3 rotationStopPosition;
    private Vector3 newZoom;
    private Quaternion newRotation;

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    async Task Update()
    {
        // Check if there is any panel opened
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            // Create asynchorous tasks for camera control
            Task moveCamera = MoveCamera();
            Task zoomCamera = ZoomCamera();
            Task rotateCamera = RotateCamera();

            // Wait until all task finished
            await Task.WhenAll(moveCamera, zoomCamera, rotateCamera);

            // Calculate movement
            transform.position = Vector3.Lerp(transform.position, newPosition, moveTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, moveTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, moveTime);
        }
    }

    // Move camera by using wsad / arrows
    private async Task MoveCamera()
    {
        #if UNITY_EDITOR
            Debug.Log("MoveCamera task started" + System.DateTime.Now.ToString());
        #endif

        await Task.Yield();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -moveSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -moveSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * moveSpeed);
        }

        // Move camera by using mouse mid button
        if (Input.GetMouseButtonDown(2))
        {
            Plane Plane = new Plane(Vector3.up, Vector3.zero);
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (Plane.Raycast(Ray, out entry))
            {
                dragStartPosition = Ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(2))
        {
            Plane Plane = new Plane(Vector3.up, Vector3.zero);
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (Plane.Raycast(Ray, out entry))
            {
                dragStopPosition = Ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragStopPosition;
            }
        }
    }

    // Zooming by using mouse scroll
    private async Task ZoomCamera()
    {
        #if UNITY_EDITOR
            Debug.Log("ZoomCamera task started" + System.DateTime.Now.ToString());
        #endif

        await Task.Yield();

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom.Set(newZoom.x, newZoom.y + zoomAmount * Input.mouseScrollDelta.y, newZoom.z);
        }
    }

    // Rotating by using leftCtrl + Mouse right Button
    private async Task RotateCamera()
    {
        #if UNITY_EDITOR
            Debug.Log("RotateCamera task started" + System.DateTime.Now.ToString());
        #endif

        await Task.Yield();
        
        if (Input.GetMouseButtonDown(1) & Input.GetKey(KeyCode.LeftControl))
        {
            rotationStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1) & Input.GetKey(KeyCode.LeftControl))
        {
            rotationStopPosition = Input.mousePosition;
            Vector3 rotationDiff = rotationStopPosition - rotationStartPosition;
            rotationStartPosition = rotationStopPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-rotationDiff.x * rotationSpeed));
        }
    }
}
