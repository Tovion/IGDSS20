using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductionBuilding : Building
{
    #region parameters
    public List<Tile.TileTypes> efficiencyScalesWithNeighboringTiles; // P
    public int minMaxNeighbors; // P
    public List<GameManager.ResourceTypes> inputResources; //P
    public GameManager.ResourceTypes outputResources; //P
    public int availableJobs;
    public float wokerEfficiency;
    #endregion

    #region Jobs
    public List<Job> jobs = new List<Job>(); // List of all available Jobs. Is populated in Start()
    #endregion


    void Start()
    {
        for (int i = 0; i < availableJobs; i++)
        {
            Job job = new Job(this);
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
        wokerEfficiency = (float)  _workers.Count / availableJobs;
        float summedHappines = 0f;
        foreach (Worker w in _workers)
        {
            summedHappines += w._happiness;
        }
        float averageHappines = 0;
        if (_workers.Count != 0)
        {
            averageHappines = summedHappines / _workers.Count;
        }
        Debug.Log("worker EFF" + wokerEfficiency * averageHappines);
        return wokerEfficiency * averageHappines;
        
    }
    public float CalculteTileEfficiency()
    {
        float tileEfficiencyValue = 1;
        var neighborTiles = tile._neighborTiles;
        float improvingNeighbors = 0;

        foreach (Tile neighborTile in neighborTiles)
        {
            if (efficiencyScalesWithNeighboringTiles.Contains(neighborTile._type))
            {
                improvingNeighbors++;
            }
        }

        if (efficiencyScalesWithNeighboringTiles.Count != 0)
        {
            tileEfficiencyValue = improvingNeighbors / minMaxNeighbors;
        }
        Debug.Log("tileeff" + tileEfficiencyValue);
        return tileEfficiencyValue;
    }
    public override void CalculateEfficiency()
    {
        float tileEfficiencyValue = CalculteTileEfficiency();
        float workerEficiencyValue = CalculateWorkerEfficiency();
        efficiencyValue = tileEfficiencyValue * workerEficiencyValue;
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
        bool allInputResourcesAvailable = true;
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
            gameManager.ChangeResourcesInWarehouse(outputResources, (float)efficiencyValue * outputCount);
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
