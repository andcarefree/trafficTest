using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using Bolt;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float moveSpeed;
    public float moveTime;
    public float rotationSpeed;

    public Vector3 newPosition;
    public Vector3 zoomAmount;
    public Vector3 dragStartPosition;
    public Vector3 dragStopPosition;
    public Vector3 rotationStartPosition;
    public Vector3 rotationStopPosition;
    public Vector3 newZoom;

    public Quaternion newRotation;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!(bool)Variables.ActiveScene.Get("isOverlayExists"))
        {
            HandleMouseMovement();
            HandleKeyboardMovement();

            HandleMouseZoom();
            HandleMouseRotation();

            transform.position = Vector3.Lerp(transform.position, newPosition, moveTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, moveTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, moveTime);
        }
    }

    void HandleKeyboardMovement()
    {
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
    }

    void HandleMouseMovement()
    {
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

    void HandleMouseZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
    }

    void HandleMouseRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rotationStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            rotationStopPosition = Input.mousePosition;
            Vector3 rotationDiff = rotationStopPosition - rotationStartPosition;
            rotationStartPosition = rotationStopPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-rotationDiff.x * rotationSpeed));
        }
    }
}
