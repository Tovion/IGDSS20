using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    private List<Job> _availableJobs = new List<Job>();
    public List<Worker> _unoccupiedWorkers = new List<Worker>();
    public List<Worker> _occupiedWorkers = new List<Worker>();

    public NavigationManager navman;

    #region MonoBehaviour
    void Update()
    {
        HandleUnoccupiedWorkers();
    }
    #endregion

    #region Methods
    public void AddJob(Job job)
    {
        _availableJobs.Add(job);
    }

    private void HandleUnoccupiedWorkers()
    {
        if (_unoccupiedWorkers.Count > 0)
        {
            if (_availableJobs.Count > 0)
            {
                var randomIndex = Random.Range(0, _availableJobs.Count - 1);
                var randomJob = _availableJobs[randomIndex];
                randomJob.AssignWorker(_unoccupiedWorkers[0]); //Also assigns worker to building
                Building  b= randomJob._building;
                //b.potentialFieldMap = navman.CalculatePotentialFields(b);
                _unoccupiedWorkers[0].SetWorkplace(b);
                /*
                Building r = _unoccupiedWorkers[0]._residence;
                r.potentialFieldMap = navman.CalculatePotentialFields(r);
                _unoccupiedWorkers[0].SetResidence(r);
                */
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
            var job = worker.GetJob();
            job.RemoveWorker(worker);
            _availableJobs.Add(job);
        }
    }
    #endregion
}
