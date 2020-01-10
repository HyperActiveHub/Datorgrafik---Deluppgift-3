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
    public bool roll;

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
        float inputSide = Input.GetAxisRaw("Horizontal");
        float inputDir = Input.GetAxisRaw("Vertical");
        Vector3 worldMove = new Vector3(inputSide, 0.0f, inputDir);

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 localMove = myCamera.transform.localToWorldMatrix.MultiplyVector(worldMove);
            localMove.y = 0;
            Move(localMove.normalized * moveSpeed);
        }
        else
        {
            Move(worldMove.normalized * moveSpeed);
        }
    }

    void Rotation()
    {
        float inputX = Input.GetAxis("Mouse X");
        float inputY = Input.GetAxis("Mouse Y");

        Rotate(Vector3.up, inputX * rotationSpeed, Space.World);

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
            Quaternion target = myCamera.transform.rotation;

            target.z = 0;
            myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, target, rotationSpeed * Time.deltaTime);

            print("resetting");

            if (Mathf.Abs(myCamera.transform.eulerAngles.z) < 0.01f)
            {
                myCamera.transform.rotation = Quaternion.Euler(myCamera.transform.rotation.x, myCamera.transform.rotation.y, 0);
                roll = false;
            }
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
