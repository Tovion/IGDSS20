using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameManager gameManager;
    bool rightMouseClickHold = false;
    private Vector3 vector3;
    int mapSize;
    int yPos;
    private void Start()
    {
         mapSize =10* gameManager.GetMapSize();
         yPos = 75;
    }
    void Update()
    {
        LeftMouseClick();
        RightMouseClick();
        ScrollWheel();
    }

    private void LeftMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
    }

    private void RightMouseClick()
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
            int xPos = (int)(-1 * Input.mousePosition.y + 500);
            int zPos = (int)Input.mousePosition.x;
            if (xPos > mapSize)
            {
                xPos = mapSize;
            }
            if (xPos < 00)
            {
                xPos = 00;
            }
            if (zPos > mapSize)
            {
                zPos = mapSize;
            }
            if (zPos < 0)
            {
                zPos = 0;
            }
            Camera.main.transform.SetPositionAndRotation(new Vector3(xPos, yPos, zPos), Quaternion.Euler(new Vector3(45, -90, 0)));
        }
    }

    private void ScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            int yPosMov;
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                yPosMov = 5;
            }
            else
            {
                yPosMov = -5;
            }
            int camY = (int) Camera.main.transform.position.y;
            if (camY > 100)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    yPosMov = 0;
                }
                else
                {
                    yPosMov = -5;
                }
            }
            if (camY < 55)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    yPosMov = 5;
                }
                else
                {
                    yPosMov = 0;
                }
            }
            yPos = camY;
            Camera.main.transform.Translate(new Vector3(0, yPosMov, 0));
        }
    }
}
