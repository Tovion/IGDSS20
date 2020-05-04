using System;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Camera maincamera;
    public GameManager gameManager;
    // Update is called once per frame
    bool rightmousedown = false;
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
        if (Input.GetMouseButtonDown(1))
        {
            rightmousedown = true;
        }
        if (Input.GetMouseButtonUp(1))
        {

            rightmousedown = false;
        }
        if (rightmousedown)
        {
            Debug.Log(Input.mousePosition);
            int xPos = (int)(-1*Input.mousePosition.y +500);
            int zPos = (int) Input.mousePosition.x;
            if(xPos > mapSize)
            {
                xPos = mapSize;
            }
            if (xPos < 50)
            {
                xPos = 50;
            }
            if (zPos > mapSize)
            {
                zPos = mapSize;
            }
            if (zPos < 0)
            {
                zPos = 0;
            }
            maincamera.transform.SetPositionAndRotation(new Vector3(xPos, yPos, zPos),Quaternion.Euler(new Vector3(45,-90,0)));

        }
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
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
            int camY = (int)maincamera.transform.position.y;
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
            if (camY < 10)
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
            maincamera.transform.Translate(new Vector3(0, yPosMov, 0));
        }
    }
}
