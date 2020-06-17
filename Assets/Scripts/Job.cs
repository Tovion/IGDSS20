public class Job
{
    public Worker _worker; //The worker occupying this job
    public Building _building; //The building offering the job

    public Job(ProductionBuilding building)
    {
        _building = building;
    }

    public void AssignWorker(Worker worker)
    {
        _worker = worker;
        _worker.SetJob(this);
        _building.WorkerAssignedToBuilding(_worker);
    }

    public void RemoveWorker(Worker worker)
    {
        _worker = worker;
        _building.WorkerRemovedFromBuilding(_worker);
    }
}
