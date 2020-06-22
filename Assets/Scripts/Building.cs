using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    #region Manager References
    public GameManager gameManager; //Reference to the GameManager
    public JobManager jobManager; //Reference to the JobManager
    #endregion
    
    #region Workers
    public List<Worker> _workers = new List<Worker>(); //List of all workers associated with this building, either for work or living
    #endregion

    #region Building Parameters
    protected float timer;
    public string type; //The type of the Building. E.g. Fishery, Lumberjack etc. for Production Buildings or Residence for Housing Buildings
    public int upkeep; //Probably both building types could have upkeep cost in the future (for Housing Building it is currently set to 0)
    public int buildCostMoney; //Probably both building types could have a fixed build cost in the future (for Housing Building it is currently set to 0)
    public int buildCostPlanks; //Probably both building types could have a fixed plank consumption (for Housing Building it is currently set to 0)
    public float efficiencyValue; //Both building types have an efficiency value that depends on different factors
    public float generationInterval; //Buildings generates something, either resources or workers at intervals
    public int outputCount; //The amount of the output (resources or workers)
    public Tile tile; //Both building types are build upon a tile
    public List<Tile.TileTypes> canBeBuiltOnTileTypes; //Both building types can only build on certain tiles (e.g. not on water etc.) 
    public int[,] potentialFieldMap;

    #endregion

    public abstract void CalculateEfficiency();

    #region Methods   
    public void WorkerAssignedToBuilding(Worker worker)
    {
        _workers.Add(worker);
        worker.SetWorkplace(this);
    }

    public void WorkerRemovedFromBuilding(Worker worker)
    {
        _workers.Remove(worker);
    }
    #endregion
}
