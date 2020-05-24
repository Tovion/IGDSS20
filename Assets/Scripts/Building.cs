using System;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameManager gameManager;
    
    #region parameters
    private float timer;
    public string type;
    public int upkeep;
    public int buildCostMoney;
    public int buildCostPlanks;
    public Tile tile;
    public float efficencyValue;
    public float ressourceGenerationInterval;
    public int outputCount;
    public List<Tile.TileTypes> canBeBuiltOnTileTypes;
    public List<Tile.TileTypes> efficencyScalesWithNighboringTiles;
    public int minMaxNeighbours;
    public List<GameManager.ResourceTypes> inputRessources;
    public GameManager.ResourceTypes outputRessources;
    #endregion

    void Update()
    {
        CallInputOutput();
    }
    
    public void CalculateEfficiency()
    {
        var neighbours = tile._neighborTiles;
        float improvingNeighbours = 0;

        foreach (Tile t in neighbours)
        {
            if(efficencyScalesWithNighboringTiles.Contains(t._type))
            {
                improvingNeighbours++;
            }
        }
      
        efficencyValue = improvingNeighbours / minMaxNeighbours;
        if (efficencyScalesWithNighboringTiles.Count == 0)
        {
            efficencyValue = 1;
        }
        if (efficencyValue> 1)
        {
            efficencyValue = 1;
        }
    }

    void CallInputOutput()
    {
        timer += Time.deltaTime;
        if (timer > ressourceGenerationInterval)
        {
            InputOutput();
            timer -= ressourceGenerationInterval;
        }
    }

    void InputOutput()
    {
        Boolean allInputResourcesAvailable = true;
        foreach (GameManager.ResourceTypes i in inputRessources)
        {
            if (!gameManager.HasResourceInWarehoues(i))
            {
                allInputResourcesAvailable = false;
            }

        }
        if (allInputResourcesAvailable)
        {
            foreach (GameManager.ResourceTypes i in inputRessources)
            {
                gameManager.ChangeResourcesInWarehouse(i, -1);
            }
            gameManager.ChangeResourcesInWarehouse(outputRessources, (float)Math.Round(efficencyValue * outputCount, 2));
        }
    }

    public void CalculateOutputResource()
    {
        switch(type){
            case "Fishery":
                outputRessources = GameManager.ResourceTypes.Fish;
                efficencyScalesWithNighboringTiles.Add(Tile.TileTypes.Water);
                break;
            case "Lumberjack":
                outputRessources = GameManager.ResourceTypes.Wood;
                efficencyScalesWithNighboringTiles.Add(Tile.TileTypes.Forest);
                break;
            case "Sawmill":
                outputRessources = GameManager.ResourceTypes.Planks;
                break;
            case "Sheep Farm":
                outputRessources = GameManager.ResourceTypes.Wool;
                efficencyScalesWithNighboringTiles.Add(Tile.TileTypes.Grass);
                break;
            case "Framework Knitters":
                outputRessources = GameManager.ResourceTypes.Clothes;
                break;
            case "Potato Farm":
                outputRessources = GameManager.ResourceTypes.Potato;
                efficencyScalesWithNighboringTiles.Add(Tile.TileTypes.Grass);
                break;
            case "Schnapps Distillery":
                outputRessources = GameManager.ResourceTypes.Schnapps;
                break;
        }
    }
}
