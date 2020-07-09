using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : Building
{
    #region Production Building Parameters
    public List<Tile.TileTypes> efficiencyScalesWithNeighboringTiles;
    public int minMaxNeighbors;
    public List<GameManager.ResourceTypes> inputResources;
    public GameManager.ResourceTypes outputResources;
    public int availableJobs;
    public float wokerEfficiency;
    public List<Job> jobs = new List<Job>(); // List of all available Jobs. Is populated in Start()
    #endregion

    void Start()
    {
        InitAvailableJobs();
    }

    private void InitAvailableJobs()
    {
        for (var i = 0; i < availableJobs; i++)
        {
            var job = new Job(this);
            jobs.Add(job);
            jobManager.AddJob(job);
        }
    }
    

    void Update()
    {
        CallInputOutput();
        CalculateEfficiency();
    }

    private float  CalculateWorkerEfficiency()
    {
        var summedHappiness = 0f;
        var averageHappiness = 0f;

        wokerEfficiency = (float)  _workers.Count / availableJobs;
    
        foreach (var worker in _workers)
        {
            summedHappiness += worker._happiness;
        }
    
        if (_workers.Count != 0)
        {
            averageHappiness = summedHappiness / _workers.Count;
        }
        return wokerEfficiency * averageHappiness;
    }
    public float CalculateTileEfficiency()
    {
        float tileEfficiencyValue = 1;
        var neighborTiles = tile._neighborTiles;
        var improvingNeighbors = 0;

        foreach (var neighborTile in neighborTiles)
        {
            if (efficiencyScalesWithNeighboringTiles.Contains(neighborTile._type))
            {
                improvingNeighbors++;
            }
        }
        if (efficiencyScalesWithNeighboringTiles.Count != 0)
        {
            
            tileEfficiencyValue =(float)improvingNeighbors /(float) minMaxNeighbors;
            Debug.Log(improvingNeighbors + " "+ tileEfficiencyValue);
        }
        return 1;
    }
    public override void CalculateEfficiency()
    {
        var tileEfficiencyValue = CalculateTileEfficiency();
        var workerEfficiencyValue = CalculateWorkerEfficiency();
        efficiencyValue = tileEfficiencyValue * workerEfficiencyValue;
    }

    private void CallInputOutput()
    {
        timer += Time.deltaTime;
        if (timer > generationInterval)
        {
            InputOutput();
            timer -= generationInterval;
        }
    }

    private void InputOutput()
    {
        var allInputResourcesAvailable = true;
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
                gameManager.ChangeResourcesInWarehouse(i, -1f);
            }
            gameManager.ChangeResourcesInWarehouse(outputResources, efficiencyValue * outputCount);
        }
    }

    public void CalculateOutputResource()
    {
            switch (type)
            {
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
