using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Range(5,30)][SerializeField] private int scrollSpeed=10;//speed at which you move the camera
    [Range(0f,1f)][SerializeField] private float zoomSpeed=0.05f;//Speed at which you can zoom
    [Range(5, 30)] [SerializeField] private int rotationSpeed = 5;//speed at which you can rotate

    //The barriers of the screen used for determining when the mouse it at an edge
    [Range(0f,1f)][SerializeField] private float topNRightBarrier=0.97f;
    [Range(0f,1f)][SerializeField] private float botNLeftBarrier=0.03f;

    //The holder of the camera
    [SerializeField] private GameObject cameraHolder;
    //Lock camera movement so that it does not annoy testing
    [SerializeField] private bool movementLocked = false;

    [Header("View constraints")]
    [SerializeField] private int xMaxConstraint=5, xMinConstraint = -5;
    [SerializeField] private int zMaxConstraint=5, zMinConstraint = -5;
    [SerializeField] private int yMaxConstraint=10, yMinConstraint = 0;


    //Keys used for keybinding (RIGHT NOW NOT IN USE)
    //Zooming
    private KeyCode zoomIn = KeyCode.Equals;
    private KeyCode zoomOut = KeyCode.Minus;
    //Rotation
    private KeyCode rotateRight = KeyCode.E;
    private KeyCode rotateLeft = KeyCode.Q;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CameraMovement();

        CameraZoom();

        CameraRotate();
    }
    private void CameraMovement()
    { 
        if (!movementLocked)
        {
            Vector3 previousPos = cameraHolder.transform.position;
            //Move up
            if (Input.mousePosition.y >= Screen.height * topNRightBarrier || Input.GetKey(KeyCode.W))
            {
                cameraHolder.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed, Space.World);
            }
            //Move down
            if (Input.mousePosition.y <= Screen.height * botNLeftBarrier || Input.GetKey(KeyCode.S))
            {
                cameraHolder.transform.Translate(Vector3.down * Time.deltaTime * scrollSpeed, Space.World);
            }
            //Move left
            if (Input.mousePosition.x <= Screen.width * botNLeftBarrier || Input.GetKey(KeyCode.A))
            {
                cameraHolder.transform.Translate(-Camera.main.transform.right * Time.deltaTime * scrollSpeed, Space.World);
            }
            //Move right
            if (Input.mousePosition.x >= Screen.width * topNRightBarrier || Input.GetKey(KeyCode.D))
            {
                cameraHolder.transform.Translate(Camera.main.transform.right * Time.deltaTime * scrollSpeed, Space.World);
            }
            //CONSTRAINTS
            //x-axis
            if (cameraHolder.transform.position.x>xMaxConstraint)
            {
                cameraHolder.transform.position=new Vector3(xMaxConstraint, cameraHolder.transform.position.y, cameraHolder.transform.position.z);
            }
            if (cameraHolder.transform.position.x < xMinConstraint)
            {
                cameraHolder.transform.position = new Vector3(xMinConstraint, cameraHolder.transform.position.y, cameraHolder.transform.position.z);
            }
            //z-axis
            if (cameraHolder.transform.position.z > zMaxConstraint)
            {
                cameraHolder.transform.position = new Vector3(cameraHolder.transform.position.x, cameraHolder.transform.position.y, zMaxConstraint);
            }
            if (cameraHolder.transform.position.z < zMinConstraint)
            {
                cameraHolder.transform.position = new Vector3(cameraHolder.transform.position.x, cameraHolder.transform.position.y, zMinConstraint);
            }
            //y-axis
            if (cameraHolder.transform.position.y > yMaxConstraint)
            {
                cameraHolder.transform.position = new Vector3(cameraHolder.transform.position.x, yMaxConstraint, cameraHolder.transform.position.z);
            }
            if (cameraHolder.transform.position.y < yMinConstraint)
            {
                cameraHolder.transform.position = new Vector3(cameraHolder.transform.position.x, yMinConstraint, cameraHolder.transform.position.z);
            }
        }
    }

    private void CameraZoom()
    {
        //Zoom in
        if (Input.GetKey(zoomIn) || Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            mainCamera.orthographicSize -= zoomSpeed;
        }
        //Zoom out
        if (Input.GetKey(zoomOut) || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            mainCamera.orthographicSize += zoomSpeed;
        }
    }

    private void CameraRotate()
    {
        //Rotate Left
        if (Input.GetKey(rotateLeft))
        {
            cameraHolder.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        //Rotate Right
        if (Input.GetKey(rotateRight))
        {
            cameraHolder.transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
    }
}
