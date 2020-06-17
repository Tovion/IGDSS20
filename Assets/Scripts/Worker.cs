using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    #region Manager References
    public JobManager _jobManager; //Reference to the JobManager
    public GameManager _gameManager;//Reference to the GameManager
    #endregion

    public float _age = 1; // The age of this worker
    private float _ageTimer;
    private float _agingIntervalInSecs;

    public float _happiness; // The happiness of this worker

    private float _consumptionTimer;
    public int _consumeIntervalInSecs; // The interval how often a worker consumes resources (fish, clothes, schnapps)
    public int _consumptionRating; // Is a one of the values (0, 1, 2, 3) and shows how much a worker is satisfied by resource consumption

    public Job _job;
    public Building _residence;

    private bool is15 = true;
    private bool is65 = true;
    private bool is101 = true;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _jobManager = GameObject.Find("JobManager").GetComponent<JobManager>();
        _agingIntervalInSecs = 15;
    }

    public void SetAge(float age)
    {
        _age = age;
    }

    public bool HasJob()
    {
        return _job != null;
    }

    // Update is called once per frame
    void Update()
    {
        Age();
        ConsumeResources();
        CalculateHappiness();
    }


    private void ConsumeResources()
    {
        _consumptionTimer += Time.deltaTime;
        if (_consumptionTimer > _consumeIntervalInSecs)
        {
            Consume();
            _consumptionTimer -= _consumeIntervalInSecs;
        }
    }

    private void Consume()
    {
        _consumptionRating = 0;
        if (_gameManager.HasResourceInWarehoues(GameManager.ResourceTypes.Fish))
        {
            _gameManager.ChangeResourcesInWarehouse(GameManager.ResourceTypes.Fish, -0.1f);
            _consumptionRating++;
        }

        if (_gameManager.HasResourceInWarehoues(GameManager.ResourceTypes.Clothes))
        {
            _gameManager.ChangeResourcesInWarehouse(GameManager.ResourceTypes.Clothes, -0.1f);
            _consumptionRating++;
        }

        if (_gameManager.HasResourceInWarehoues(GameManager.ResourceTypes.Schnapps))
        {
            _gameManager.ChangeResourcesInWarehouse(GameManager.ResourceTypes.Clothes, -0.1f);
            _consumptionRating++;
        }
    }

    private void CalculateHappiness()
    {
        int hasJob = 0;
        if (HasJob())
        {
            hasJob = 1;
        }
        _happiness = (float)(_consumptionRating+ hasJob) / 4;



    }

    private void Age()
    {
        //TODO: Implement a life cycle, where a Worker ages by 1 year every 15 real seconds.
        //When becoming of age, the worker enters the job market, and leaves it when retiring.
        //Eventually, the worker dies and leaves an empty space in his home. His Job occupation is also freed up.
        UpdateAge();

        if (_age > 3 && is15)
        {
            BecomeOfAge();
            is15 = false;
        }

        if (_age > 67 && is65)
        {
            Retire();
            is65 = false;
        }

        if (_age > 100 && is101)
        {
            Die();
            is101 = false;
        }
    }

    private void UpdateAge()
    {
        _ageTimer += Time.deltaTime;
        if (_ageTimer > _agingIntervalInSecs)
        {
            _age++;
            _ageTimer -= _agingIntervalInSecs;
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
}
