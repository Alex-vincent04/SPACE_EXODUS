using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign the enemy prefab in the inspector
    public int maxEnemies = 10; // Maximum number of enemies per wave
    public float spawnInterval = 1.5f; // Time between spawns
    public float waveDuration = 10f; // Time before all enemies are removed
    public float minRange = -1000f;
    public float maxRange = 1000f;
    public float minRangeZ = -1000f;
    public float maxRangeZ = 1000f;
    public float minRangeY = -1000f;
    public float maxRangeY = 1000f;

    private List<GameObject> activeEnemies = new List<GameObject>(); // List to track active enemies
    private bool spawning = false;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            spawning = true;

            for (int i = 0; i < maxEnemies; i++)
            {
                if (activeEnemies.Count < maxEnemies)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            spawning = false;

            // Wait for the wave duration, then destroy all enemies
            yield return new WaitForSeconds(waveDuration);
            ClearEnemies();

            // Wait before the next wave starts
            yield return new WaitForSeconds(2f);
        }
    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(minRange, maxRange);
        float randomZ = Random.Range(minRangeZ, maxRangeZ);
        float randomY = Random.Range(minRangeY, maxRangeY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    void ClearEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
    }
}
