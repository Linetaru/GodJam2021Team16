using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [Header("Spawner Settings")]
    [SerializeField] private List<SpawningEntity> entitiesOfThisSpawner;
    [SerializeField] private bool showSpawner = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        print(DayNightCycleManager.current);
    }

    void Start()
    {
        _spriteRenderer.enabled = showSpawner;
    }

    public void SpawnEnemy()
    {
        print(DayNightCycleManager.current.getCurrentDay());
        int currentDay = DayNightCycleManager.current.getCurrentDay();

        List<SpawningEntity> possibleSpawning = new List<SpawningEntity>();
        foreach (SpawningEntity entity in entitiesOfThisSpawner)
        {
            if (entity.dayOfSpawn <= currentDay)
                possibleSpawning.Add(entity);
        }

        int selectedEntityToSpawn = Random.Range(0, possibleSpawning.Count);

        Instantiate(possibleSpawning[selectedEntityToSpawn].entityToSpawn, transform.position, transform.rotation);
    }
}
