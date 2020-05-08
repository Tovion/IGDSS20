using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public Texture2D mapTexture;

    public void Start()
    {
        BuildMap();
    }

    private void BuildMap()
    {
        var pix = GetPixels();
        var xOffset = 0f;
        var zOffset = 0f;
        var counter = 0;

        for (var i = 0; i < pix.Length; i++)
        {
            if (NextRow(i))
            {
                xOffset += 8.5f;
                zOffset = counter % 2 == 0 ? 5f : 0f;
                counter++;
            }

            // maxColor is the highest RGB value of a pixel [0;1]
            var maxColor = Math.Max(Math.Max(pix[i].r, pix[i].g), pix[i].b);

            // adapt the prefab height
            var yOffset = maxColor * 50; 

            PlacePrefab(maxColor, xOffset, yOffset, zOffset);
            zOffset += 10;


        }
    }

    private Color[] GetPixels()
    {
        return mapTexture.GetPixels(
            Mathf.FloorToInt(0),
            Mathf.FloorToInt(0),
            Mathf.FloorToInt(mapTexture.width),
            Mathf.FloorToInt(mapTexture.height));
    }

    private void PlacePrefab(float maxColor, float xOffset, float yOffset, float zOffset)
    {
        if (maxColor <= 0)
        {
            InstantiatePrefab("WaterTile", xOffset, yOffset, zOffset);
        }
        else if (maxColor > 0 && maxColor <= 0.2)
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
    }

    private void InstantiatePrefab(string prefabName, float xPos, float yPos, float zPos)
    {
        Instantiate(
            (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefabName + ".prefab", typeof(GameObject)),
            new Vector3(xPos, yPos, zPos), Quaternion.identity);
    }

    private bool NextRow(int pixelIndex)
    {
        return (pixelIndex != 0) && (pixelIndex % (int) mapTexture.width) == 0;
    }


    public int GetMapSize()
    {
        return mapTexture.width; // width = height (since the textures are square)
    }
}
