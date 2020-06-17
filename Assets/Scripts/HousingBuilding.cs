using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HousingBuilding : Building
{
    public const int MAX_CAPACITY = 10;

    // Start is called before the first frame update
    void Start()
    {
        var worker1GameObject = Instantiate(
            (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Workers/Worker.prefab", typeof(GameObject)),
            new Vector3(0, 0, 0), Quaternion.identity);

        var worker2GameObject = Instantiate(
            (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Workers/Worker.prefab", typeof(GameObject)),
            new Vector3(0, 0, 0), Quaternion.identity);

        Worker worker1 = worker1GameObject.GetComponent<Worker>();
        Worker worker2 = worker2GameObject.GetComponent<Worker>();

        worker1.SetAge(15f);
        worker2.SetAge(15f);

        worker1._residence = this;
        worker2._residence = this;

        WorkerAssignedToBuilding(worker1);
        WorkerAssignedToBuilding(worker2);
    }

    // Update is called once per frame
    void Update()
    {
        ProduceWorker();
        CalculateEfficiency();
    }

    private void ProduceWorker()
    {
        timer += Time.deltaTime;
        if (timer*efficiencyValue > generationInterval)
        {
            if (_workers.Count < MAX_CAPACITY)
            {
                SpawnNewWorker();
            }

            timer -= generationInterval;
        }
    }

    private void SpawnNewWorker()
    {
        var workerGameObject = Instantiate(
            (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Workers/Worker.prefab", typeof(GameObject)),
            new Vector3(0, 0, 0), Quaternion.identity);

        Worker worker = workerGameObject.GetComponent<Worker>();
        worker._residence = this;

        WorkerAssignedToBuilding(worker);
    }

    public override void CalculateEfficiency()
    {
        float summedHappines = 0f;
        foreach (Worker w in _workers)
        {
            summedHappines +=  w._happiness;
        }
        efficiencyValue = summedHappines /_workers.Count;
    }
}
