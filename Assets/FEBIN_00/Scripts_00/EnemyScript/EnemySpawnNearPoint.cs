using UnityEngine;
using System.Collections;

public class EnemySpawnerNearPlayer : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign enemy prefab in the Inspector
    public Transform player; // Assign player GameObject in the Inspector
    public int enemyCount = 5; // Number of enemies to spawn
    public float spawnRange = 10f; // Spawn radius around the player
    public float spawnInterval = 2f; // Time between spawns

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionNearPlayer();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPositionNearPlayer()
    {
        if (player == null) return Vector3.zero; // Prevent errors if player is missing

        // Generate a random position around the player
        Vector2 randomCircle = Random.insideUnitCircle * spawnRange;
        Vector3 spawnPos = new Vector3(player.position.x + randomCircle.x, player.position.y, player.position.z + randomCircle.y);
        return spawnPos;
    }
}
