using UnityEditor;
using UnityEngine;

public class HousingBuilding : Building
{
    #region Housing Building Parameters
    public const int MAX_CAPACITY = 10;
    #endregion

    void Start()
    {
        var worker1 = InstantiateWorker().GetComponent<Worker>();
        var worker2 = InstantiateWorker().GetComponent<Worker>();

        worker1.SetAge(15f);
        worker2.SetAge(15f);

        worker1.SetResidence(this);
        worker2.SetResidence(this);

        WorkerAssignedToBuilding(worker1);
        WorkerAssignedToBuilding(worker2);
    }

    void Update()
    {
        ProduceWorker();
        CalculateEfficiency();
    }

    private GameObject InstantiateWorker()
    {
        return Instantiate(
            (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Workers/Worker.prefab", typeof(GameObject)), transform.position, Quaternion.identity);
    }

    private void ProduceWorker()
    {
        timer += Time.deltaTime;
        if (timer*efficiencyValue > generationInterval)
        {
            if (RoomAvailable())
            {
                SpawnNewWorker();
            }

            timer -= generationInterval;
        }
    }

    private bool RoomAvailable()
    {
        return _workers.Count < MAX_CAPACITY;
    }

    private void SpawnNewWorker()
    {
        var worker = InstantiateWorker().GetComponent<Worker>();
        worker.SetAge(1f);
        worker.SetResidence(this);
        WorkerAssignedToBuilding(worker);
    }

    public override void CalculateEfficiency()
    {
        float summedHappiness = 0f;
        foreach (var worker in _workers)
        {
            summedHappiness += worker._happiness;
        }
        efficiencyValue = summedHappiness /_workers.Count;
    }
}
