using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentMoney;

    #region Enumerations
    public enum ResourceTypes { None, Fish, Wood, Planks, Wool, Clothes, Potato, Schnapps }; //Enumeration of all available resource types.
    #endregion

    #region Map generation
    public Texture2D mapTexture;
    private Tile[,] _tileMap; //2D array of all spawned tiles
    #endregion

    #region Resources
    private Dictionary<ResourceTypes, float> _resourcesInWarehouse;
    #endregion

    #region Buildings
    public Building[] _buildingPrefabs; //References to the building prefabs
    public int _selectedBuildingPrefabIndex = 0; //The current index used for choosing a prefab to spawn from the _buildingPrefabs list
    public List<Building> buildings;
    #endregion

    #region Upkeep
    private float timer;
    private const float WAIT_TIME_IN_SECONDS = 60f;
    #endregion

    #region Resources for inspector view
    //A representation of _resourcesInWarehouse, broken into individual floats. Only for display in inspector, will be removed and replaced with UI later
    [SerializeField]
    private float _ResourcesInWarehouse_Fish;
    [SerializeField]
    private float _ResourcesInWarehouse_Wood;
    [SerializeField]
    private float _ResourcesInWarehouse_Planks;
    [SerializeField]
    private float _ResourcesInWarehouse_Wool;
    [SerializeField]
    private float _ResourcesInWarehouse_Clothes;
    [SerializeField]
    private float _ResourcesInWarehouse_Potato;
    [SerializeField]
    private float _ResourcesInWarehouse_Schnapps;
    #endregion

    void Start()
    {
        InitCamera();
        PopulateResourceDictionary();
        InitTileMap();
        BuildMap();
    }

    void Update()
    {
        HandleKeyboardInput();
        UpdateInspectorNumbersForResources();
        Upkeep();
    }

    private void InitCamera()
    {
        Camera.main.transform.SetPositionAndRotation(
            new Vector3(150f, 100f, 80f),
            Quaternion.Euler(new Vector3(60f, -90f, 0f)));
    }

    //Makes the resource dictionary usable by populating the values and keys
    private void PopulateResourceDictionary()
    {
        //The warehouse dictionary holds a number of stored resources for every ResourceType
        _resourcesInWarehouse = new Dictionary<ResourceTypes, float>
        {
            { ResourceTypes.None, 0 },
            { ResourceTypes.Fish, 0 },
            { ResourceTypes.Wood, 0 },
            { ResourceTypes.Planks, 0 },
            { ResourceTypes.Wool, 0 },
            { ResourceTypes.Clothes, 0 },
            { ResourceTypes.Potato, 0 },
            { ResourceTypes.Schnapps, 0 }
        };
    }

    private void InitTileMap()
    {
        _tileMap = new Tile[mapTexture.width, mapTexture.height];
    }

    private void BuildMap()
    {
        var pix = GetPixels();
        var xOffset = 0f;
        var zOffset = 0f;
        var counter = 0;
        var mapSize = GetMapSize();
        for (var i = 0; i < pix.Length; i++)
        {
            if (NextRow(i))
            {
                xOffset += 8.5f;
                zOffset = counter % 2 == 0 ? 5f : 0f;
                counter++;
            }

            // maxColor is the maximum RGB value of a pixel [0;1]
            var maxColor = Math.Max(Math.Max(pix[i].r, pix[i].g), pix[i].b);

            // adapt the prefab height
            var yOffset = maxColor * 50;

            Tile tile= PlacePrefab(maxColor, xOffset, yOffset, zOffset);
            tile._coordinateHeight = i % mapSize;
            tile._coordinateWidth =counter;
            _tileMap[counter, i % mapSize] = tile;
            zOffset += 10;
        }
        for (var i = 0; i< mapSize; i++)
        {
            for (var j = 0; j < mapSize; j++)
            {
                _tileMap[i, j]._neighborTiles = FindNeighborsOfTile(_tileMap[i, j]);
            }
        }
    }

    private Color[] GetPixels()
    {
        return mapTexture.GetPixels(
            Mathf.FloorToInt(0),
            Mathf.FloorToInt(0),
            Mathf.FloorToInt(mapTexture.width),
            Mathf.FloorToInt(mapTexture.height));
    }

    private Tile PlacePrefab(float maxColor, float xOffset, float yOffset, float zOffset)
    {
        if (maxColor <= 0)
        {
            return InstantiatePrefab("WaterTile", xOffset, yOffset, zOffset);
        }
        else if (maxColor > 0 && maxColor <= 0.2)
        {
            return InstantiatePrefab("SandTile", xOffset, yOffset, zOffset);
        }
        else if (maxColor > 0.2 && maxColor <= 0.4)
        {
            return InstantiatePrefab("GrassTile", xOffset, yOffset, zOffset);
        }
        else if (maxColor > 0.4 && maxColor <= 0.6)
        {
            return InstantiatePrefab("ForestTile", xOffset, yOffset, zOffset);
        }
        else if (maxColor > 0.6 && maxColor <= 0.8)
        {
            return InstantiatePrefab("StoneTile", xOffset, yOffset, zOffset);
        }
        else if (maxColor > 0.8)
        {
            return InstantiatePrefab("MountainTile", xOffset, yOffset, zOffset);
        }
        return new Tile();
    }

    private Tile InstantiatePrefab(string prefabName, float xPos, float yPos, float zPos)
    {
        var prefabGameObject = Instantiate(
            (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefabName + ".prefab", typeof(GameObject)),
            new Vector3(xPos, yPos, zPos), Quaternion.identity);
        return prefabGameObject.GetComponent<Tile>();
    }

    private bool NextRow(int pixelIndex)
    {
        return (pixelIndex != 0) && (pixelIndex % (int) mapTexture.width) == 0;
    }

    public int GetMapSize()
    {
        return mapTexture.width; // width = height (since the textures are square)
    }

    public void ChangeResourcesInWarehouse(ResourceTypes resource, float changingNumber)
    {
        _resourcesInWarehouse[resource] += changingNumber; 
    }

    //Sets the index for the currently selected building prefab by checking key presses on the numbers 1 to 0
    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedBuildingPrefabIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedBuildingPrefabIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _selectedBuildingPrefabIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _selectedBuildingPrefabIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _selectedBuildingPrefabIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _selectedBuildingPrefabIndex = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _selectedBuildingPrefabIndex = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _selectedBuildingPrefabIndex = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _selectedBuildingPrefabIndex = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _selectedBuildingPrefabIndex = 9;
        }
    }

    //Updates the visual representation of the resource dictionary in the inspector. Only for debugging
    private void UpdateInspectorNumbersForResources()
    {
        _ResourcesInWarehouse_Fish = _resourcesInWarehouse[ResourceTypes.Fish];
        _ResourcesInWarehouse_Wood = _resourcesInWarehouse[ResourceTypes.Wood];
        _ResourcesInWarehouse_Planks = _resourcesInWarehouse[ResourceTypes.Planks];
        _ResourcesInWarehouse_Wool = _resourcesInWarehouse[ResourceTypes.Wool];
        _ResourcesInWarehouse_Clothes = _resourcesInWarehouse[ResourceTypes.Clothes];
        _ResourcesInWarehouse_Potato = _resourcesInWarehouse[ResourceTypes.Potato];
        _ResourcesInWarehouse_Schnapps = _resourcesInWarehouse[ResourceTypes.Schnapps];
    }

    private void Upkeep()
    {
        timer += Time.deltaTime;
        if (timer > WAIT_TIME_IN_SECONDS)
        {
            var overallUpkeep = 100;
            foreach (Building building in buildings)
            {
                overallUpkeep -= building.upkeep;
            }
            currentMoney += overallUpkeep;
            timer -= WAIT_TIME_IN_SECONDS;
        }
    }

    //Checks if there is at least one material for the queried resource type in the warehouse
    public bool HasResourceInWarehoues(ResourceTypes resource)
    {
        return _resourcesInWarehouse[resource] >= 1;
    }

    //Is called by MouseManager when a tile was clicked
    //Forwards the tile to the method for spawning buildings
    public void TileClicked(int height, int width)
    {
        Tile tile = _tileMap[height, width];
        PlaceBuildingOnTile(tile);
    }

    //Checks if the currently selected building type can be placed on the given tile and then instantiates an instance of the prefab
    private void PlaceBuildingOnTile(Tile tile)
    {
        //if there is building prefab for the number input
        if (_selectedBuildingPrefabIndex < _buildingPrefabs.Length)
        {
            Building building = _buildingPrefabs[_selectedBuildingPrefabIndex];
               
            if (BuildingPossible(building, tile))
            { 
                currentMoney -= building.buildCostMoney;
                ChangeResourcesInWarehouse(ResourceTypes.Planks, -building.buildCostPlanks);
                building.tile = tile;
                building.CalculateOutputResource();
                building.CalculateEfficiency();
                building.gameManager = this;
                PlaceBuilding(building, tile);
                buildings.Add(building);
                tile._building = building;
            }
        }
    }

    private bool BuildingPossible(Building building, Tile tile)
    {
        return HasNoBuilding(tile) && 
               CanBeBuiltOnTileType(building, tile) &&
               HasEnoughMoney(building) &&
               HasEnoughPlanksInWarehouse(building);
    }

    private bool HasNoBuilding(Tile tile)
    {
        return tile._building == null;
    }

    private bool CanBeBuiltOnTileType(Building building, Tile tile)
    {
        return building.canBeBuiltOnTileTypes.Contains(tile._type);
    }

    private bool HasEnoughMoney(Building building)
    {
        return building.buildCostMoney <= currentMoney;
    }

    private bool HasEnoughPlanksInWarehouse(Building building)
    {
        return building.buildCostPlanks <= _resourcesInWarehouse[ResourceTypes.Planks];
    }

    private void PlaceBuilding(Building building, Tile tile)
    {
        var buildPosX = tile.transform.position.x + building.transform.position.x;
        var buildPosY = tile.transform.position.y + building.transform.position.y;
        var buildPosZ = tile.transform.position.z + building.transform.position.z;

        Instantiate(
            (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + building.type + ".prefab", typeof(GameObject)),
            new Vector3(buildPosX, buildPosY, buildPosZ), building.transform.rotation);
    }

    //Returns a list of all neighbors of a given tile
    private List<Tile> FindNeighborsOfTile(Tile t)
    {
        List<Tile> result = new List<Tile>();
        var limit = GetMapSize();
        var width = t._coordinateWidth;
        var height = t._coordinateHeight;

        if (width-1 > 0)
        {
            result.Add(_tileMap[width - 1, height]);
        }
        if (width + 1 < limit)
        {
            result.Add(_tileMap[width + 1, height]);
        }
        if (height + 1 < limit)
        {
            result.Add(_tileMap[width, height+1]);
        }
        if (height - 1 > 0)
        {
            result.Add(_tileMap[width, height-1]);
        }
        
        if(width % 2 == 0)
        {
            if (width - 1 > 0 && height - 1 > 0)
            {
                result.Add(_tileMap[width - 1, height - 1]);
            }
            if (width + 1 < limit && height - 1 > 0)
            {
                result.Add(_tileMap[width + 1, height - 1]);
            }
        }
        if (width % 2 != 0)
        {
            if (width + 1 < limit && height + 1 < limit)
            {
                result.Add(_tileMap[width + 1, height + 1]);
            }

            if (width - 1 > 0 && height + 1 < limit)
            {
                result.Add(_tileMap[width - 1, height + 1]);
            }
        }
        return result;
    }
}
