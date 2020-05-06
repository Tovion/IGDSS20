﻿using System;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D mapTexture;

    void Start()
    {
        BuildMap();
    }

    public int GetMapSize()
    {
        return mapTexture.width;
    }
    
    void BuildMap()
    {
        Color[] pix = GetPixels();

        int counter = 0;
        float zOffset = 0f;  
        float xOffset = 0f;

        for (int i = 0; i < pix.Length; i++)
        {
            if (i != 0 && i % (int) mapTexture.width == 0)
            {
                xOffset += 8.5f;
                zOffset = counter % 2 == 0 ? 5f : 0f;
                counter++;
            }

            float maxColor = Math.Max(Math.Max(pix[i].r, pix[i].g), pix[i].b);
            float yOffset = maxColor*50;

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
            int x = Mathf.FloorToInt(0);
            int y = Mathf.FloorToInt(0);
            int width = Mathf.FloorToInt(mapTexture.width);
            int height = Mathf.FloorToInt(mapTexture.height);

            return mapTexture.GetPixels(x, y, width, height);
        }

        void InstantiatePrefab(String prefabName, float xPos, float yPos, float zPos)
        {
            Instantiate(
                (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefabName + ".prefab", typeof(GameObject)),
                new Vector3(xPos, yPos, zPos), Quaternion.identity);
        }
    }
}
