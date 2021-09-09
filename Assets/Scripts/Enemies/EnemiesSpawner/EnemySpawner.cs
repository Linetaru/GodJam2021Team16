using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [Header("Spawner Settings")]
    [SerializeField] private List<SpawningEntity> entitiesOfThisSpawner;
    [SerializeField] private bool showSpawner = true;
    [SerializeField] private float spawningRange = 5f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _spriteRenderer.enabled = showSpawner;
    }

    public void SpawnEnemy()
    {
        int currentDay = DayNightCycleManager.current.getCurrentDay();

        List<SpawningEntity> possibleSpawning = new List<SpawningEntity>();
        foreach (SpawningEntity entity in entitiesOfThisSpawner)
        {
            if (entity.dayOfSpawn <= currentDay)
                possibleSpawning.Add(entity);
        }

        int selectedEntityToSpawn = Random.Range(0, possibleSpawning.Count);

        GameObject enemySpawned = Instantiate(possibleSpawning[selectedEntityToSpawn].entityToSpawn, new Vector3(Random.Range(transform.position.x - (spawningRange / 2), transform.position.x + (spawningRange / 2)), Random.Range(transform.position.y - (spawningRange / 2), transform.position.y + (spawningRange / 2)), 1), transform.rotation);
        enemySpawned.transform.SetParent(this.transform);
        enemySpawned.name = possibleSpawning[selectedEntityToSpawn].entityToSpawn.name;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawningRange, spawningRange, 1));
    }
}
