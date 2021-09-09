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

    public void MakeSpawnerSpawnAEntity()
    {
        int spawnerIndex = Random.Range(0, _spawners.Length);
        if (_spawners.Length > 0)
        {
            EnemySpawner spawner = _spawners[spawnerIndex].GetComponent<EnemySpawner>();
            spawner.SpawnEnemy();

        }
    }
}
