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
    public float efficiencyValue;
    public float resourceGenerationInterval;
    public int outputCount;
    public List<Tile.TileTypes> canBeBuiltOnTileTypes;
    public List<Tile.TileTypes> efficiencyScalesWithNeighboringTiles;
    public int minMaxNeighbors;
    public List<GameManager.ResourceTypes> inputResources;
    public GameManager.ResourceTypes outputResources;
    #endregion

    void Update()
    {
        CallInputOutput();
    }
    
    public void CalculateEfficiency()
    {
        efficiencyValue = 1;
        var neighborTiles = tile._neighborTiles;
        float improvingNeighbors = 0;
        
        foreach (Tile neighborTile in neighborTiles)
        {
            if(efficiencyScalesWithNeighboringTiles.Contains(neighborTile._type))
            {
                improvingNeighbors++;
            }
        }

        if (efficiencyScalesWithNeighboringTiles.Count != 0)
        {
            efficiencyValue = improvingNeighbors / minMaxNeighbors;
        }
    }

    void CallInputOutput()
    {
        timer += Time.deltaTime;
        if (timer > resourceGenerationInterval)
        {
            InputOutput();
            timer -= resourceGenerationInterval;
        }
    }

    void InputOutput()
    {
        Boolean allInputResourcesAvailable = true;
        foreach (GameManager.ResourceTypes i in inputResources)
        {
            if (!gameManager.HasResourceInWarehoues(i))
            {
                allInputResourcesAvailable = false;
            }

        }
        if (allInputResourcesAvailable)
        {
            foreach (GameManager.ResourceTypes i in inputResources)
            {
                gameManager.ChangeResourcesInWarehouse(i, -1);
            }
            gameManager.ChangeResourcesInWarehouse(outputResources, (float)Math.Round(efficiencyValue * outputCount, 2));
        }
    }

    public void CalculateOutputResource()
    {
        switch(type){
            case "Fishery":
                outputResources = GameManager.ResourceTypes.Fish;
                efficiencyScalesWithNeighboringTiles.Add(Tile.TileTypes.Water);
                break;
            case "Lumberjack":
                outputResources = GameManager.ResourceTypes.Wood;
                efficiencyScalesWithNeighboringTiles.Add(Tile.TileTypes.Forest);
                break;
            case "Sawmill":
                outputResources = GameManager.ResourceTypes.Planks;
                break;
            case "Sheep Farm":
                outputResources = GameManager.ResourceTypes.Wool;
                efficiencyScalesWithNeighboringTiles.Add(Tile.TileTypes.Grass);
                break;
            case "Framework Knitters":
                outputResources = GameManager.ResourceTypes.Clothes;
                break;
            case "Potato Farm":
                outputResources = GameManager.ResourceTypes.Potato;
                efficiencyScalesWithNeighboringTiles.Add(Tile.TileTypes.Grass);
                break;
            case "Schnapps Distillery":
                outputResources = GameManager.ResourceTypes.Schnapps;
                break;
        }
    }
}
