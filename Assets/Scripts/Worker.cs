using UnityEngine;

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
    private Building _residence; //Reference to the building where the worker lives
    public float _happiness; //The happiness of this worker

    void Start()
    {
        InitManagers();
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

    public void SetJob(Job job)
    {
        _job = job;
    }

    public Job GetJob()
    {
        return _job;
    }
}
