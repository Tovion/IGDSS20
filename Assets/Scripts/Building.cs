using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        callInputOutput();
    }
    public GameManager gameManager;
    //parameters
    #region parameters
    float timer = 0;
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
    public void CalculateEfficency()
    {
        var neighbours = tile._neighborTiles;
        float improvingNeihgbours = 0;
        foreach (Tile t in neighbours)
        {
            if(efficencyScalesWithNighboringTiles.Contains(t._type))
            {
                improvingNeihgbours++;
            }
        }
        Debug.Log(improvingNeihgbours/minMaxNeighbours );
        efficencyValue = improvingNeihgbours / minMaxNeighbours;

        if (efficencyValue> 1)
        {
            efficencyValue = 1;
        }
    }
    void InputOutput()
    {
        foreach(GameManager.ResourceTypes i in inputRessources)
        {
            if (gameManager.HasResourceInWarehoues(i))
            {
                gameManager.ChangeResourcesInWarehouse(i, -1);
            }
        }
        gameManager.ChangeResourcesInWarehouse(outputRessources,(int) efficencyValue * outputCount);
    }
    void callInputOutput()
    {
        timer += Time.deltaTime;
        if (timer > ressourceGenerationInterval)
        {
            InputOutput();
            timer -= ressourceGenerationInterval;
        }

    }
    public void calculateOutputResource()
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
