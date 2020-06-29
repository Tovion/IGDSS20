using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Worker : MonoBehaviour
{
    #region Manager References
    public JobManager _jobManager; //Reference to the JobManager
    public GameManager _gameManager;//Reference to the GameManager
    #endregion

    #region Age
    private float _agingTimer;
    public float _age; //The age of this worker (starts by 1)
    public float _agingIntervalInSecs; //A Worker ages by 1 year every 15 real seconds

    private bool flag_worker = true;
    private bool flag_retiree = true;
    private bool flag_dead = true;
    #endregion

    #region Consumption
    private float _consumptionTimer;
    public int _consumptionIntervalInSecs; //The interval how often a worker consumes resources (fish, clothes, schnapps)
    private int _consumptionRating; //Is a one of the values (0, 1, 2, 3) and shows how much a worker is satisfied by resource consumption. 0 = no consumption, 3 = all consuming resources are available
    private const float CONSUMPTION_AMOUNT = 0.1f;
    #endregion

    private Job _job; //Reference to the job if he has one
    public  Building _residence; //Reference to the building where the worker lives
    public Building _workplace; //Reference to the building where the worker works
    public float _happiness; //The happiness of this worker
    float timer = 0f;
    float commuteInterval = 10f;

    void Start()
    {
        InitManagers();

        _gameManager.calculatePotentialFields(_residence);
        Debug.Log(_residence.potentialFieldMap.Length);
    }

    private void InitManagers()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _jobManager = GameObject.Find("JobManager").GetComponent<JobManager>();
    }


    void Update()
    {
        Age();
        ConsumeResources();
        CalculateHappiness();
        moveWorker();
    }
    void moveWorker()
    {
        if (_workplace != null)
        {
            if (_workplace.potentialFieldMap != null)
            {
                {

                    if (transform.position == _residence.transform.position)
                    {
                        timer += Time.deltaTime;
                        if (timer > commuteInterval)
                        {
                            move(_residence, _workplace);
                            timer -= commuteInterval;
                        }
                    }
                    else
                    {
                        timer += Time.deltaTime;
                        if (timer > commuteInterval)
                        {
                            move(_workplace, _residence);
                            timer -= commuteInterval;
                        }
                    }
                }
            }
        }

    }
    private void physicalMove(Tuple<int, int> workerPos)
    {
        Tile[,] _tileMap = _gameManager._tileMap;
        Tile tile = _tileMap[workerPos.Item1, workerPos.Item2];
        this.transform.position = new Vector3(tile.transform.position.x,tile.transform.position.y,tile.transform.position.z);
    }
    private void move(Building origin,Building goal)
    {
        int[,] mapOrigin = origin.potentialFieldMap;
        int[,] mapGoal = goal.potentialFieldMap;
        int goalx = 0;
        int goaly = 0;
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                if (mapGoal[i, j] == 0)
                {
                    goalx = i;
                    goaly = j;
                }
            }
        }
        int orix = 0;
        int oriy = 0;
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                if (mapOrigin[i, j] == 0)
                {
                    orix = i;
                    oriy = j;
                }
            }
        }
        Tuple<int, int> workerPos = new Tuple<int, int>(orix, oriy);
        Tuple<int, int> goalPos = new Tuple<int, int>(goalx, goaly);
        while (workerPos != goalPos)
        {
            workerPos = findBestNextStep(workerPos, mapGoal);
            physicalMove(workerPos);
        }
        //TODO breitensuche um besten weg von orix und oriy zu goalx und goaly zu finden.


    }

    private Tuple<int, int> findBestNextStep(Tuple<int, int> workerPos, int[,] mapGoal)
    {
        int x = workerPos.Item1;
        int y = workerPos.Item2;
        int mapsize = _gameManager.GetMapSize();
        int lowestPot = 1000;

        Tuple<int, int> newWorkerPos = new Tuple<int, int>(x, y);
        if (x < mapsize)
        {
            if(mapGoal[x+1,y] < lowestPot)
            {
                lowestPot = mapGoal[x + 1, y];
                Tuple<int, int> newWorkerPos2 = new Tuple<int, int>(x+1, y);
                newWorkerPos = newWorkerPos2;
            }
        }
        if (x > 0 )
        {
            if (mapGoal[x - 1, y] < lowestPot)
            {
                lowestPot = mapGoal[x - 1, y];
                Tuple<int, int> newWorkerPos2 = new Tuple<int, int>(x - 1, y);
                newWorkerPos = newWorkerPos2;
            }
        }

        if (y < mapsize - 1 )
        {
            if (mapGoal[x, y+1] < lowestPot)
            {
                lowestPot = mapGoal[x, y+1];
                Tuple<int, int> newWorkerPos2 = new Tuple<int, int>(x, y+1);
                newWorkerPos = newWorkerPos2;
            }
        }
        if (y > 0)
        {
            if (mapGoal[x, y - 1] < lowestPot)
            {
                lowestPot = mapGoal[x, y - 1];
                Tuple<int, int> newWorkerPos2 = new Tuple<int, int>(x, y - 1);
                newWorkerPos = newWorkerPos2;
            }
        }

        if (y > 0 && x > 0)
        {
            if (mapGoal[x-1, y - 1] < lowestPot)
            {
                lowestPot = mapGoal[x-1, y - 1];
                Tuple<int, int> newWorkerPos2 = new Tuple<int, int>(x-1, y - 1);
                newWorkerPos = newWorkerPos2;
            }
        }

        if (y > 0 && x < mapsize - 1)
        {
            if (mapGoal[x-1, y + 1] < lowestPot)
            {
                lowestPot = mapGoal[x-1, y + 1];
                Tuple<int, int> newWorkerPos2 = new Tuple<int, int>(x-1, y + 1);
                newWorkerPos = newWorkerPos2;
            }
        }

        if (y < mapsize - 1 && x > 0 )
        {
            if (mapGoal[x+1, y - 1] < lowestPot)
            {
                lowestPot = mapGoal[x+1, y - 1];
                Tuple<int, int> newWorkerPos2 = new Tuple<int, int>(x+1, y - 1);
                newWorkerPos = newWorkerPos2;
            }
        }

        if (y < mapsize - 1 && x < mapsize - 1 )
        {
            if (mapGoal[x+1, y + 1] < lowestPot)
            {
                lowestPot = mapGoal[x+1, y +1 ];
                Tuple<int, int> newWorkerPos2 = new Tuple<int, int>(x+1, y + 1);
                newWorkerPos = newWorkerPos2;
            }
        }
        return newWorkerPos;
    }

    private void ConsumeResources()
    {
        _consumptionTimer += Time.deltaTime;
        if (_consumptionTimer > _consumptionIntervalInSecs)
        {
            Consume();
            _consumptionTimer -= _consumptionIntervalInSecs;
        }
    }

    private void Consume()
    {
        _consumptionRating = 0;
        if (_gameManager.HasResourceInWarehoues(GameManager.ResourceTypes.Fish))
        {
            _gameManager.ChangeResourcesInWarehouse(GameManager.ResourceTypes.Fish, -CONSUMPTION_AMOUNT);
            _consumptionRating++;
        }

        if (_gameManager.HasResourceInWarehoues(GameManager.ResourceTypes.Clothes))
        {
            _gameManager.ChangeResourcesInWarehouse(GameManager.ResourceTypes.Clothes, -CONSUMPTION_AMOUNT);
            _consumptionRating++;
        }

        if (_gameManager.HasResourceInWarehoues(GameManager.ResourceTypes.Schnapps))
        {
            _gameManager.ChangeResourcesInWarehouse(GameManager.ResourceTypes.Schnapps, -CONSUMPTION_AMOUNT);
            _consumptionRating++;
        }
    }

    private void CalculateHappiness()
    {
        var hasJobRating = 0;
        if (HasJob())
        {
            hasJobRating = 1;
        }
        _happiness = (float)(_consumptionRating + hasJobRating) / 4;
    }

    private void Age()
    {
        UpdateAge();

        if (_age > 14 && flag_worker)
        {
            BecomeOfAge();
            flag_worker = false;
        }

        if (_age > 64 && flag_retiree)
        {
            Retire();
            flag_retiree = false;
        }

        if (_age > 100 && flag_dead)
        {
            Die();
            flag_dead = false;
        }
    }

    private void UpdateAge()
    {
        _agingTimer += Time.deltaTime;
        if (_agingTimer > _agingIntervalInSecs)
        {
            _age++;
            _agingTimer -= _agingIntervalInSecs;
        }
    }

    public void BecomeOfAge()
    {
        _jobManager.RegisterWorker(this);
    }

    private void Retire()
    {
        _jobManager.RemoveWorker(this);
    }

    private void Die()
    {
        _residence.WorkerRemovedFromBuilding(this);
        Destroy(this.gameObject, 1f);
    }

    public bool HasJob()
    {
        return _job != null;
    }

    public void SetAge(float age)
    {
        _age = age;
    }

    public void SetResidence(Building residence)
    {
        _residence = residence;
    }
    public void SetWorkplace(Building workplace)
    {
        _workplace = workplace;
        _gameManager.calculatePotentialFields(_workplace);
        Debug.Log(_workplace.potentialFieldMap.Length);

    }

    public void SetJob(Job job)
    {
        _job = job;
    }

    public Job GetJob()
    {
        return _job;
    }
}
