using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera myCamera;

    [Header("Movement and Rotation")]
    [SerializeField]
    float rotationSpeed = 10;
    [SerializeField]
    float moveSpeed = 100;
    [Header("Camera Zoom")]
    [SerializeField]
    float zoomFOV = 30;
    [SerializeField]
    float zoomSpeed = 10;

    float defaultZoom;
    bool roll;

    void Start()
    {
        myCamera = GetComponent<Camera>();
        defaultZoom = myCamera.fieldOfView;
    }

    void Update()
    {
        Rotation();

        Movement();

        Zoom();
    }

    void Rotate(Vector3 axis, float angle, Space space)
    {
        myCamera.transform.Rotate(axis, angle * Time.deltaTime, space);
    }

    void Move(Vector3 moveVector)
    {
        myCamera.transform.localPosition += moveVector * Time.deltaTime;
    }

    void Movement()
    {
        float inputSide = Input.GetAxis("Horizontal") * moveSpeed;
        float inputDir = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 worldMove = new Vector3(inputSide, 0.0f, inputDir);

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 localMove = myCamera.transform.localToWorldMatrix.MultiplyVector(worldMove);
            localMove.y = 0;
            Move(localMove);
        }
        else
        {
            Move(worldMove);
        }
    }

    void Rotation()
    {
        float inputX = Input.GetAxis("Mouse X");
        Rotate(Vector3.up, inputX * rotationSpeed, Space.World);

        float inputY = Input.GetAxis("Mouse Y");
        Rotate(Vector3.right, -inputY * rotationSpeed, Space.Self);

        if (Input.GetMouseButtonDown(1))
        {
            roll = true;
        }

        if (Input.GetMouseButton(1))
        {
            Rotate(Vector3.forward, inputX * rotationSpeed, Space.Self);
        }
        else if (roll)
        {
            if (Mathf.Abs(myCamera.transform.eulerAngles.z) > 0.001f)
            {
                Quaternion target = myCamera.transform.rotation;
                target.z = 0;
                //target = myCamera.transform.localToWorldMatrix
                Matrix4x4 rotMatrix = Matrix4x4.Rotate(target);
                target = rotMatrix.rotation;
                myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, target, rotationSpeed * Time.deltaTime);
            }
            else
                roll = false;
        }
    }

    void Zoom()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (Mathf.Abs(myCamera.fieldOfView - zoomFOV) > 0.01f)
                myCamera.fieldOfView = Mathf.MoveTowards(myCamera.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
        }
        else
        {
            if (Mathf.Abs(myCamera.fieldOfView - defaultZoom) > 0.01f)
                myCamera.fieldOfView = Mathf.MoveTowards(myCamera.fieldOfView, defaultZoom, zoomSpeed * Time.deltaTime);
        }
    }
}
