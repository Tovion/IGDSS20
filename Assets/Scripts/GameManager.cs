using System;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D mapTexture;

    public void Start()
    {
        BuildMap();
    }

    public int GetMapSize()
    {
        return mapTexture.width;
    }
    
    void BuildMap()
    {
        var pix = GetPixels();
        var counter = 0;
        var zOffset = 0f;  
        var xOffset = 0f;

        for (int i = 0; i < pix.Length; i++)
        {
            if (i != 0 && i % (int) mapTexture.width == 0)
            {
                xOffset += 8.5f;
                zOffset = counter % 2 == 0 ? 5f : 0f;
                counter++;
            }

            var maxColor = Math.Max(Math.Max(pix[i].r, pix[i].g), pix[i].b);
            var yOffset = maxColor*50;

            if (maxColor <= 0)
            {
                InstantiatePrefab("WaterTile", xOffset, yOffset, zOffset);
            } else if(maxColor > 0 && maxColor <= 0.2)
            {
                InstantiatePrefab("SandTile", xOffset, yOffset, zOffset);
            }
            else if (maxColor > 0.2 && maxColor <= 0.4)
            {
                InstantiatePrefab("GrassTile", xOffset, yOffset, zOffset);
            }
            else if (maxColor > 0.4 && maxColor <= 0.6)
            {
                InstantiatePrefab("ForestTile", xOffset, yOffset, zOffset);
            }
            else if (maxColor > 0.6 && maxColor <= 0.8)
            {
                InstantiatePrefab("StoneTile", xOffset, yOffset, zOffset);
            }
            else if (maxColor > 0.8)
            {
                InstantiatePrefab("MountainTile", xOffset, yOffset, zOffset);
            }

            zOffset += 10;
        }

        Color[] GetPixels()
        {
            return mapTexture.GetPixels(
                Mathf.FloorToInt(0), 
                Mathf.FloorToInt(0), 
                Mathf.FloorToInt(mapTexture.width), 
                Mathf.FloorToInt(mapTexture.height));
        }

        void InstantiatePrefab(string prefabName, float xPos, float yPos, float zPos)
        {
            Instantiate(
                (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefabName + ".prefab", typeof(GameObject)),
                new Vector3(xPos, yPos, zPos), Quaternion.identity);
        }
    }
}
