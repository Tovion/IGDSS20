using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MouseManager : MonoBehaviour
{
    public GameManager gameManager;

    private Camera cam;
    private int mapSize;
    private bool rightMouseClickHold;

    // for mouse movement
    private const float MOVE_SPEED = 0.05f;
    private Vector3 currentMousePosition;
    private float xPos;
    private float yPos;
    private float zPos;
  

    private void Start()
    { 
        cam = Camera.main;
        mapSize = 10 * gameManager.GetMapSize();
        rightMouseClickHold = false;
    }
    void Update()
    {
        ClickOnPrefab();
        CameraMovement();
        CameraScrollWheel();
    }

    // left mouse click
    private void ClickOnPrefab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
    }

    // hold right mouse click 
    private void CameraMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseClickHold = true;
            currentMousePosition = Input.mousePosition; // store the mouse pos. for each right click
            xPos = cam.transform.position.x;
            yPos = cam.transform.position.y;
            zPos = cam.transform.position.z;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rightMouseClickHold = false;
        }

        if (rightMouseClickHold)
        {
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        float xOffset= ((currentMousePosition.x - Input.mousePosition.x) * -1) * MOVE_SPEED;
        float yOffset = (currentMousePosition.y - Input.mousePosition.y) * MOVE_SPEED;

        // also possible with Time.deltaTime but it needs more fine-tuning;
        //float xFactor = ((currentMousePosition.x - Input.mousePosition.x) * -1) * Time.deltaTime * 2f;
        //float yFactor = (currentMousePosition.y - Input.mousePosition.y) * Time.deltaTime * 2f;

        // right boundary
        if (xPos > mapSize) { xPos = mapSize; }

        // left boundary
        if (xPos < 0) { xPos = 0; }

        // up boundary
        if (zPos > mapSize) { zPos = mapSize; }

        // bottom boundary
        if (zPos < 0) { zPos = 0; }

        cam.transform.SetPositionAndRotation(
            new Vector3(xPos += yOffset, yPos, zPos += xOffset),
            Quaternion.Euler(new Vector3(60, -90, 0)));
    }

    private void CameraScrollWheel()
    {
        const float MAX_ZOOM_IN = 50f;
        const float MAX_ZOOM_OUT = 120f;
        var yPos = cam.transform.position.y;
        
        if (yPos < MAX_ZOOM_OUT && Input.GetAxis("Mouse ScrollWheel") < 0f) // zoom in
        {
            cam.transform.Translate(new Vector3(0, 0, -10));
        } 

        if (yPos > MAX_ZOOM_IN && Input.GetAxis("Mouse ScrollWheel") > 0f) // zoom out
        {
            cam.transform.Translate(new Vector3(0, 0, 10));
        } 
    }

}
