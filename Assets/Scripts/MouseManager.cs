using UnityEngine;

public class MouseManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("RIGHT CLICK");
        }
    }
}
