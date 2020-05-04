using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Camera maincamera;

    // Update is called once per frame
    bool rightmousedown = false;
    private Vector3 vector3;

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
            int xPos = (int)(-1*Input.mousePosition.y +200);
            int zPos = (int) Input.mousePosition.x;
            int yPos = 75;
            maincamera.transform.SetPositionAndRotation(new Vector3(xPos, yPos, zPos),Quaternion.Euler(new Vector3(45,-90,0)));

        }
    }
}
