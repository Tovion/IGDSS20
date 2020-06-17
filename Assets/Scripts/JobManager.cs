using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    private List<Job> _availableJobs = new List<Job>();
    public List<Worker> _unoccupiedWorkers = new List<Worker>();
    public List<Worker> _occupiedWorkers = new List<Worker>();

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleUnoccupiedWorkers();

        //Debug.Log("Number of available jobs: " +_availableJobs.Count + " " + " Number of unoccupied workers: " + _unoccupiedWorkers.Count);
    }
    #endregion


    #region Methods

    public void AddJob(Job job)
    {
        _availableJobs.Add(job);
    }

    private void HandleUnoccupiedWorkers()
    {
        //TODO: What should be done with unoccupied workers?
        if (_unoccupiedWorkers.Count > 0)
        {
            if (_availableJobs.Count > 0)
            {
                var randomIndex = UnityEngine.Random.Range(0, _availableJobs.Count - 1);
                var randomJob = _availableJobs[randomIndex];

                randomJob.AssignWorker(_unoccupiedWorkers[0]); // Also assigned worker to building

                _availableJobs.RemoveAt(randomIndex);
                _occupiedWorkers.Add(_unoccupiedWorkers[0]);
                _unoccupiedWorkers.RemoveAt(0);

            }
        }
    }

    public void RegisterWorker(Worker worker)
    {
        _unoccupiedWorkers.Add(worker);
    }



    public void RemoveWorker(Worker worker)
    {
        _unoccupiedWorkers.Remove(worker);
        _occupiedWorkers.Remove(worker);

        if (worker.HasJob())
        {
            Job job = worker._job;

            Debug.Log(job);

            job.RemoveWorker(worker);
            _availableJobs.Add(job);
        }
    }


    #endregion
}
