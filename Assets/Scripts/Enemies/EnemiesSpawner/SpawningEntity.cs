using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawner/New Enemy", fileName = "Entity_")]
public class SpawningEntity : ScriptableObject
{
    // Prefab of the entity to summon
    public GameObject entityToSpawn;

    // day from which entity will spawn
    public int dayOfSpawn = 0;

}
