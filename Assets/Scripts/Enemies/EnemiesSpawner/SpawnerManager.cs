using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    public static SpawnerManager current;
    GameObject[] _spawners;

    private void Awake()
    {
        current = this;
        _spawners = GameObject.FindGameObjectsWithTag("Spawner");
    }

    int _numberOfTriesToSpawnMax = 10;

    public void MakeSpawnerSpawnAEntity()
    {
        if (_spawners.Length > 0)
        {
            int spawnerIndex = Random.Range(0, _spawners.Length);

            EnemySpawner spawner = _spawners[spawnerIndex].GetComponent<EnemySpawner>();
            int numberOfTries = 0;
            while (!spawner.SpawnEnemy() && numberOfTries < _numberOfTriesToSpawnMax)
            {
                spawnerIndex = Random.Range(0, _spawners.Length);
                spawner = _spawners[spawnerIndex].GetComponent<EnemySpawner>();
                numberOfTries++;
            }
            numberOfTries = 0;

        }
    }
}
