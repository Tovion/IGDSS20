using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavigationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    public GameManager gm;
    // Update is called once per frame
    void Update()
    {
        
    }
    public void calculatePotentialFields(Building building)
    {
        Tile[,] tileMap = gm._tileMap;
        int mapsize = gm.GetMapSize();
        int[,] potentialFieldMap = new int[mapsize, mapsize];
        int x = building.tile._coordinateWidth;
        int y = building.tile._coordinateHeight;
        List<string> usedPositions = new List<string>();
        int ourposx = 0;
        int ourposy = 0;
        for (int i = 0; i < mapsize; i++)
        {
            for (int j = 0; j< mapsize; j++)
            {
                if (tileMap[i,j]._coordinateHeight == y && tileMap[i, j]._coordinateWidth == x)
                {
                    ourposx = i;
                    ourposy = j;
                }
            }
        }
        potentialFieldMap[ourposx, ourposy] = 0;
        usedPositions.Add(ourposx+""+ ourposy);

        potentialFieldMap = calculatePotentials(tileMap, ourposx, ourposy, potentialFieldMap, usedPositions);
        while (usedPositions.Count < mapsize * mapsize)
        {
            int lowestvalue = 10000;
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (potentialFieldMap[i,j] != 0&& potentialFieldMap[i,j] < lowestvalue && !usedPositions.Contains(i + "" + j))
                    {
                        lowestvalue = potentialFieldMap[i, j];
                        ourposx = i;
                        ourposy = j;
                    }
                }
            }
            potentialFieldMap = calculatePotentials(tileMap, ourposx, ourposy, potentialFieldMap, usedPositions);
            usedPositions.Add(ourposx + "" + ourposy);

        }
        building.potentialFieldMap = potentialFieldMap;
    }
    int[,] calculatePotentials(Tile[,] tileMap, int x , int y, int[,] fieldMap, List<string> usedPos)
    {
        int mapsize = gm.GetMapSize();
        int currentPot = fieldMap[x, y];
        if(x < mapsize-1 && !usedPos.Contains((x+1) + ""+y))
        {
            fieldMap[x+1, y] = currentPot + calculateWeight(tileMap[x + 1, y]); 
        }
        if (x > 0 && !usedPos.Contains((x - 1) + "" + y))
        {
            fieldMap[x - 1, y] = currentPot + calculateWeight(tileMap[x - 1, y]); 
        }

        if (y < mapsize-1 && !usedPos.Contains(x  + "" + (y+1)))
        {
            fieldMap[x, y+1] = currentPot + calculateWeight(tileMap[x, y+1]); 
        }
        if (y > 0 && !usedPos.Contains(x  + "" + (y-1)))
        {
            fieldMap[x, y-1] = currentPot + calculateWeight(tileMap[x, y-1]); 
        }

        if (y > 0 && x>0 && !usedPos.Contains((x - 1) + "" + (y-1)))
        {
            fieldMap[x-1, y - 1] = currentPot + calculateWeight(tileMap[x-1, y - 1]); 
        }

        if (y > 0 && x < mapsize-1 && !usedPos.Contains((x + 1) + "" + (y-1)))
        {
            fieldMap[x + 1, y - 1] = currentPot + calculateWeight(tileMap[x + 1, y - 1]); 
        }

        if (y < mapsize-1 && x > 0 && !usedPos.Contains((x - 1) + "" + (y+1)))
        {
            fieldMap[x - 1, y + 1] = currentPot + calculateWeight(tileMap[x - 1, y + 1]); 
        }

        if (y < mapsize -1&& x < mapsize-1 && !usedPos.Contains((x + 1) + "" + (y+1)))
        {
            fieldMap[x + 1, y + 1] = currentPot + calculateWeight(tileMap[x + 1, y + 1]); 
        }
        return fieldMap;
    }
    int calculateWeight(Tile tile)
    {
        switch (tile._type)
        {
            case Tile.TileTypes.Water:
                return 30;

            case Tile.TileTypes.Stone:
                return 1;

            case Tile.TileTypes.Sand:
                return 2;

            case Tile.TileTypes.Mountain:
                return 3;

            case Tile.TileTypes.Grass:
                return 1;

            case Tile.TileTypes.Forest:
                return 2;

        }
        return -1;
    }
}
