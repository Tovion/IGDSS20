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

    private int temp = 0;

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
        CalculateWorkerEfficiency();
    }

    private void CalculateWorkerEfficiency()
    {
        // TODO

        if (_workers.Count != temp)
        {
            temp = _workers.Count;

            wokerEfficiency = (float)  _workers.Count / availableJobs;

            //efficiencyValue = efficiencyValue * wokerEfficiency;
        }
    }

    public override void CalculateEfficiency()
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
