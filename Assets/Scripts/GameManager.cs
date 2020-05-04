using System;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D sourceTex;
    public Rect sourceRect;

    void Start()
    {
        AdaptMapSize();
        BuildMap();
    }

    /**
     *  Sets the size of the map rectangle to the number from the filename
     *  e.g. Heighmap_16 so the size (width / height) will be 16px
     */
    void AdaptMapSize()
    {
        string[] heightMapName = sourceTex.name.Split('_');
        int mapSize = Int32.Parse(heightMapName[1]);
        sourceRect.width = mapSize;
        sourceRect.height = mapSize;
    }

    void BuildMap()
    {
        Color[] pix = GetPixels();

        int counter = 0;
        float zOffset = 0f;  
        float xOffset = 0f;

        for (int i = 0; i < pix.Length; i++)
        {
            if (i != 0 && i % (int) sourceRect.width == 0)
            {
                xOffset += 8.5f;
                zOffset = counter % 2 == 0 ? 5f : 0f;
                counter++;
            }

            float maxColor = Math.Max(Math.Max(pix[i].r, pix[i].g), pix[i].b);
            float yOffset;

            if (maxColor <= 0)
            {
                yOffset = 0f;
                InstantiatePrefab("WaterTile", xOffset, yOffset, zOffset);
            } else if(maxColor > 0 && maxColor <= 0.2)
            {
                yOffset = UnityEngine.Random.Range(0.01f, 2.5f);
                InstantiatePrefab("SandTile", xOffset, yOffset, zOffset);
            }
            else if (maxColor > 0.2 && maxColor <= 0.4)
            {
                yOffset = UnityEngine.Random.Range(3.5f, 7f);
                InstantiatePrefab("GrassTile", xOffset, yOffset, zOffset);
            }
            else if (maxColor > 0.4 && maxColor <= 0.6)
            {
                yOffset = UnityEngine.Random.Range(8f, 12f);
                InstantiatePrefab("ForestTile", xOffset, yOffset, zOffset);
            }
            else if (maxColor > 0.6 && maxColor <= 0.8)
            {
                yOffset = UnityEngine.Random.Range(10f, 16f);
                InstantiatePrefab("StoneTile", xOffset, yOffset, zOffset);
            }
            else if (maxColor > 0.8)
            {
                yOffset = UnityEngine.Random.Range(18f, 20f);
                InstantiatePrefab("MountainTile", xOffset, yOffset, zOffset);
            }

            zOffset += 10;
        }

        Color[] GetPixels()
        {
            int x = Mathf.FloorToInt(sourceRect.x);
            int y = Mathf.FloorToInt(sourceRect.y);
            int width = Mathf.FloorToInt(sourceRect.width);
            int height = Mathf.FloorToInt(sourceRect.height);

            return sourceTex.GetPixels(x, y, width, height);
        }

        void InstantiatePrefab(String prefabName, float xPos, float yPos, float zPos)
        {
            Instantiate(
                (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefabName + ".prefab", typeof(GameObject)),
                new Vector3(xPos, yPos, zPos), Quaternion.identity);
        }
    }
}
