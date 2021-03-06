﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour
{
    #region Attributes
    public TileTypes _type; //The type of the tile
    public Building _building; //The building on this tile
    public List<Tile> _neighborTiles; //List of all surrounding tiles. Generated by GameManager
    public int _coordinateHeight; //The coordinate on the y-axis on the tile grid (not world coordinates)
    public int _coordinateWidth; //The coordinate on the x-axis on the tile grid (not world coordinates)
    #endregion

    #region Enumerations
    public enum TileTypes { Empty, Water , Sand, Grass, Forest, Stone, Mountain }; //Enumeration of all available tile types. Can be addressed from other scripts by calling Tile.Tiletypes
    #endregion

    //This class acts as a data container and has no functionality

    public GameObject edge0; // bottom right edge, adjacent neighbor (x+1, y-1)
    public GameObject edge1; // bottom left edge,  adjacent neighbor (x+1, y-1)
    public GameObject edge2; // left edge,         adjacent neighbor (x  , y-1)
    public GameObject edge3; // top left edge,     adjacent neighbor (y-1, y-1)
    public GameObject edge4; // top right edge,    adjacent neighbor (x-1, y  )
    public GameObject edge5; // right edge,        adjacent neighbor (x  , y+1)

    void Start()
    {
        int len = gameObject.transform.childCount;

        edge0 = gameObject.transform.GetChild(len-6).gameObject;
        edge1 = gameObject.transform.GetChild(len-5).gameObject;
        edge2 = gameObject.transform.GetChild(len-4).gameObject; 
        edge3 = gameObject.transform.GetChild(len-3).gameObject;
        edge4 = gameObject.transform.GetChild(len-2).gameObject;
        edge5 = gameObject.transform.GetChild(len-1).gameObject;

        // TODO Check from neighboring tiles if they have the same type. 
        Debug.Log("This Tile position width: " + _coordinateWidth + " height: " + _coordinateHeight);
       
        for (int i = 0; i < _neighborTiles.Count; i++)
        {
           // Debug.Log("------> Neighbor:  " + _neighborTiles[i]._type + ", position: " + _neighborTiles[i]._coordinateWidth + "." + _neighborTiles[i]._coordinateHeight);
        }


        foreach (var neighborTile in _neighborTiles)
        {
            if (_type.Equals(neighborTile._type))
            {
                int x = neighborTile._coordinateWidth;
                int y = neighborTile._coordinateHeight;
                if (IsEven(_coordinateWidth))
                {
                    if (x + 1 == _coordinateWidth && y + 1 == _coordinateHeight) // edge0 and edge3 are adjacent
                    {
                        //neighborTile.edge0.SetActive(false);
                        edge3.SetActive(false);
                    }
                    if (x + 1 == _coordinateWidth && y == _coordinateHeight) // edge1 and edge4 are adjacent
                    {
                        //neighborTile.edge1.SetActive(false);
                        edge4.SetActive(false);
                    }
                    if (x + 1 == _coordinateWidth && y - 1 == _coordinateHeight)
                    {
                        //neighborTile.edge1.SetActive(false);
                        edge4.SetActive(false);
                    }


                    if (x == _coordinateWidth && y + 1 == _coordinateHeight) // 
                    {
                        //neighborTile.edge5.SetActive(false);
                        edge2.SetActive(false);
                    }
                    if (x == _coordinateWidth && y - 1 == _coordinateHeight) // 
                    {
                        //neighborTile.edge2.SetActive(false);
                        edge5.SetActive(false);
                    }


                    if (x - 1 == _coordinateWidth && y + 1 == _coordinateHeight) // 
                    {
                        //neighborTile.edge2.SetActive(false);
                        edge1.SetActive(false);
                    }
                    if (x - 1 == _coordinateWidth && y == _coordinateHeight) // 
                    {
                        //neighborTile.edge2.SetActive(false);
                        edge0.SetActive(false);
                    }
                    if (x - 1 == _coordinateWidth && y - 1 == _coordinateHeight) // 
                    {
                        //neighborTile.edge2.SetActive(false);
                        edge0.SetActive(false);
                    }
                }
                else
                {
                    if (x + 1 == _coordinateWidth && y + 1 == _coordinateHeight) // edge0 and edge3 are adjacent
                    {
                        //neighborTile.edge0.SetActive(false);
                        edge3.SetActive(false);
                    }
                    if (x + 1 == _coordinateWidth && y == _coordinateHeight) // edge1 and edge4 are adjacent
                    {
                        //neighborTile.edge1.SetActive(false);
                        edge3.SetActive(false);
                    }
                    if (x + 1 == _coordinateWidth && y - 1 == _coordinateHeight)
                    {
                        //neighborTile.edge1.SetActive(false);
                        edge4.SetActive(false);
                    }


                    if (x == _coordinateWidth && y + 1 == _coordinateHeight) // 
                    {
                        //neighborTile.edge5.SetActive(false);
                        edge2.SetActive(false);
                    }
                    if (x == _coordinateWidth && y - 1 == _coordinateHeight) // 
                    {
                        //neighborTile.edge2.SetActive(false);
                        edge5.SetActive(false);
                    }


                    if (x - 1 == _coordinateWidth && y + 1 == _coordinateHeight) // 
                    {
                        //neighborTile.edge2.SetActive(false);
                        edge1.SetActive(false);
                    }
                    if (x - 1 == _coordinateWidth && y == _coordinateHeight) // 
                    {
                        //neighborTile.edge2.SetActive(false);
                        edge1.SetActive(false);
                    }
                    if (x - 1 == _coordinateWidth && y - 1 == _coordinateHeight) // 
                    {
                        //neighborTile.edge2.SetActive(false);
                        edge0.SetActive(false);
                    }

                }
            }




        }
    }
    private bool IsEven(int number)
    {
        return number % 2 == 0;
    }

    private bool IsOdd(int number)
    {
        return number % 2 != 0;
    }
}
