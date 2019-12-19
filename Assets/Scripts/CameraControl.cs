using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera myCamera;

    [SerializeField]
    float rotationSpeed = 10;

    [SerializeField]
    float moveSpeed = 100;

    void Start()
    {
        myCamera = GetComponent<Camera>();
    }
    
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X");
        myCamera.transform.Rotate(Vector3.up, inputX * rotationSpeed * Time.deltaTime, Space.World);

        float inputY = Input.GetAxis("Mouse Y");
        myCamera.transform.Rotate(Vector3.right, -inputY * rotationSpeed * Time.deltaTime, Space.Self);

        float inputSide = Input.GetAxis("Horizontal") * moveSpeed;
        float inputDir = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 worldMove = new Vector3(inputSide, 0.0f, inputDir);

        Vector3 localMove = myCamera.transform.localToWorldMatrix.MultiplyVector(worldMove);
        localMove.y = 0;
        myCamera.transform.localPosition += localMove * Time.deltaTime;

    }
}
