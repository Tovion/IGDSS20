using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    #region Manager References
    public GameManager gameManager;
    public JobManager jobManager; //Reference to the JobManager
    #endregion
    
    #region Workers
    public List<Worker> _workers = new List<Worker>(); //List of all workers associated with this building, either for work or living
    #endregion

    #region parameters
    protected float timer; // B
    public string type; // B
    public int upkeep; // B
    public int buildCostMoney;  // B
    public int buildCostPlanks; // B
    public float efficiencyValue; // B
    public float generationInterval; // B
    public int outputCount; // B
    public Tile tile; // B
    public List<Tile.TileTypes> canBeBuiltOnTileTypes; //  public Tile tile; // B
    #endregion

    public abstract void CalculateEfficiency();

    #region Methods   
    public void WorkerAssignedToBuilding(Worker w)
    {
        _workers.Add(w);
    }

    public void WorkerRemovedFromBuilding(Worker w)
    {
        _workers.Remove(w);
    }
    #endregion
}
