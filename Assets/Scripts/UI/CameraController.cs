using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [field : SerializeField]
    public float MoveSpeed { get; set; }

    [field : SerializeField]
    public float MoveTime { get; set; }

    [field : SerializeField]
    public float RotationSpeed { get; set; }

    [field : SerializeField]
    public float ZoomAmount { get; set; }

    [SerializeField]
    private Transform cameraTransform;
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
            transform.position = Vector3.Lerp(transform.position, newPosition, MoveTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, MoveTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, MoveTime);
        }
    }

    // Move camera by using wsad / arrows
    private async Task MoveCamera()
    {
        await Task.Yield();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -MoveSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -MoveSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * MoveSpeed);
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
        await Task.Yield();

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom.Set(newZoom.x, newZoom.y + ZoomAmount * Input.mouseScrollDelta.y, newZoom.z);
        }
    }

    // Rotating by using leftCtrl + Mouse right Button
    private async Task RotateCamera()
    {
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
            newRotation *= Quaternion.Euler(Vector3.up * (-rotationDiff.x * RotationSpeed));
        }
    }
}
