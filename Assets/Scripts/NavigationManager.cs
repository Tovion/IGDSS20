using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public GameManager gm;
    
    public int[,] CalculatePotentialFields(Building building)
    {
        var tileMap = gm._tileMap;
        var mapsize = gm.GetMapSize();
        var potentialFieldMap = new int[mapsize, mapsize];
        var x = building.tile._coordinateWidth;
        var y = building.tile._coordinateHeight;
        var usedPositions = new List<string>();
        var ourposx = 0;
        var ourposy = 0;
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
        usedPositions.Add(ourposx+" "+ ourposy);

        potentialFieldMap = CalculatePotentials(tileMap, ourposx, ourposy, potentialFieldMap, usedPositions);
        
        while (usedPositions.Count < mapsize * mapsize)
        {
            var lowestValue = 100000;
            for (var i = 0; i < mapsize; i++)
            {
                for (var j = 0; j < mapsize; j++)
                {
                    if (potentialFieldMap[i,j] != 0 && potentialFieldMap[i,j] < lowestValue && !usedPositions.Contains(i + " " + j))
                    {
                        lowestValue = potentialFieldMap[i, j];
                        ourposx = i;
                        ourposy = j;
                    }
                }
            }
            potentialFieldMap = CalculatePotentials(tileMap, ourposx, ourposy, potentialFieldMap, usedPositions);
            usedPositions.Add(ourposx + " " + ourposy);

        }
        return potentialFieldMap;
    }

    private int[,] CalculatePotentials(Tile[,] tileMap, int x , int y, int[,] fieldMap, List<string> usedPos)
    {
        var mapsize = gm.GetMapSize();
        var currentPot = fieldMap[x, y];
        if(x < mapsize-1 && !usedPos.Contains((x+1) + " "+y))
        {
            fieldMap[x+1, y] = currentPot + CalculateWeight(tileMap[x + 1, y]); 
        }
        if (x > 0 && !usedPos.Contains((x - 1) + " " + y))
        {
            fieldMap[x - 1, y] = currentPot + CalculateWeight(tileMap[x - 1, y]); 
        }

        if (y < mapsize-1 && !usedPos.Contains(x  + " " + (y+1)))
        {
            fieldMap[x, y+1] = currentPot + CalculateWeight(tileMap[x, y+1]); 
        }
        if (y > 0 && !usedPos.Contains(x  + " " + (y-1)))
        {
            fieldMap[x, y-1] = currentPot + CalculateWeight(tileMap[x, y-1]); 
        }

        if (y > 0 && x>0 && !usedPos.Contains((x - 1) + " " + (y-1)))
        {
            fieldMap[x-1, y - 1] = currentPot + CalculateWeight(tileMap[x-1, y - 1]); 
        }

        if (y > 0 && x < mapsize-1 && !usedPos.Contains((x + 1) + " " + (y-1)))
        {
            fieldMap[x + 1, y - 1] = currentPot + CalculateWeight(tileMap[x + 1, y - 1]); 
        }

        if (y < mapsize-1 && x > 0 && !usedPos.Contains((x - 1) + " " + (y+1)))
        {
            fieldMap[x - 1, y + 1] = currentPot + CalculateWeight(tileMap[x - 1, y + 1]); 
        }

        if (y < mapsize -1&& x < mapsize-1 && !usedPos.Contains((x + 1) + " " + (y+1)))
        {
            fieldMap[x + 1, y + 1] = currentPot + CalculateWeight(tileMap[x + 1, y + 1]); 
        }
        
        return fieldMap;
    }
    private int CalculateWeight(Tile tile)
    {
        switch (tile._type)
        {
            case Tile.TileTypes.Water: return 30;
            case Tile.TileTypes.Stone: return 1;
            case Tile.TileTypes.Sand: return 2;
            case Tile.TileTypes.Mountain: return 3; 
            case Tile.TileTypes.Grass: return 1;
            case Tile.TileTypes.Forest: return 2;
        }
        return -1;
    }
}
