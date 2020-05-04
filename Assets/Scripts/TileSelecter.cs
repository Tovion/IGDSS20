using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelecter : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        string tilename = other.tag;
        Debug.Log(tilename + "hit");
    }
}
