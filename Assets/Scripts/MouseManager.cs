﻿using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameManager gameManager;

    private Camera cam;
    private int mapSize;
    private bool rightMouseClickHold;

    // for mouse movement
    private const float MOVE_SPEED = 2f;
    private Vector2 cachedMousePosition;
    private Vector3 cachedCameraPosition;

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

    // right mouse click 
    private void CameraMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseClickHold = true;
            cachedMousePosition = Input.mousePosition;
            cachedCameraPosition = cam.transform.position;
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
        var xOffset = (cachedMousePosition.x - Input.mousePosition.x) * -1 * Time.deltaTime * MOVE_SPEED;
        var yOffset = (cachedMousePosition.y - Input.mousePosition.y) * Time.deltaTime * MOVE_SPEED;

        // change to xCamPos + yOffset and zCamPos + xOffset to disable auto movement
        var newXPos = cachedCameraPosition.x += yOffset;
        var newZPos = cachedCameraPosition.z += xOffset;

        newXPos = Mathf.Clamp(newXPos, 0, mapSize);
        newZPos = Mathf.Clamp(newZPos, 0, mapSize);

        cam.transform.SetPositionAndRotation(
            new Vector3(newXPos, cachedCameraPosition.y, newZPos),
            Quaternion.Euler(new Vector3(60, -90, 0)));
    }

    private void CameraScrollWheel()
    {
        const float MAX_ZOOM_IN = 60f;
        const float MAX_ZOOM_OUT = 120f;
        var yPos = cam.transform.position.y;
        
        if (yPos < MAX_ZOOM_OUT && Input.GetAxis("Mouse ScrollWheel") < 0f) // zoom in
        {
            cam.transform.Translate(new Vector3(0, 0, -10 * Time.deltaTime * 20f));
        } 

        if (yPos > MAX_ZOOM_IN && Input.GetAxis("Mouse ScrollWheel") > 0f) // zoom out
        {
            cam.transform.Translate(new Vector3(0, 0, 10 * Time.deltaTime * 20f));
        } 
    }
}
