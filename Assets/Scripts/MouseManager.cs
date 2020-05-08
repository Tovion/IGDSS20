using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameManager gameManager;

    private Camera cam;
    private int mapSize;
    private bool rightMouseClickHold;

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
        }

        if (Input.GetMouseButtonUp(1))
        {
            rightMouseClickHold = false;
        }

        if (rightMouseClickHold)
        {
            //Debug.Log(Input.mousePosition);
            var xPos = (int)(-1 * Input.mousePosition.y + 500);
            var zPos = (int)Input.mousePosition.x;
         
            // right boundary
            if (xPos > mapSize) { xPos = mapSize; }

            // left boundary
            if (xPos < 0) { xPos = 0; }             

            // up boundary
            if (zPos > mapSize) { zPos = mapSize; } 

            // bottom boundary
            if (zPos < 0) { zPos = 0; }            
            
            cam.transform.SetPositionAndRotation(
                new Vector3(xPos, cam.transform.position.y, zPos), 
                Quaternion.Euler(new Vector3(60, -90, 0)));
        }
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
