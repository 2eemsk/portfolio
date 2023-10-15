using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public BoxCollider spawnRange;
    public int numberOfEnemies;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Generate random position within the box collider bounds
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnRange.bounds.min.x, spawnRange.bounds.max.x),
                Random.Range(spawnRange.bounds.min.y, spawnRange.bounds.max.y),
                Random.Range(spawnRange.bounds.min.z, spawnRange.bounds.max.z)
            );

            // Instantiate the enemy prefab at the random position
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
