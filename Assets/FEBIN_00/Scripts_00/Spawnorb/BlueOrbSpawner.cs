using UnityEngine;

public class BlueOrbSpawner : MonoBehaviour
{
    public GameObject orbPrefab; // Assign your Orb prefab in the Inspector

    void Start()
    {
        SpawnOrbs();
    }

    void SpawnOrbs()
    {
        // Find all GameObjects tagged "Spawner"
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");

        foreach (GameObject spawnPoint in spawnPoints)
        {
            Instantiate(orbPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
