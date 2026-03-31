using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // Assign your asteroid prefab in the Inspector
    public int numberOfAsteroids = 100; // How many asteroids to spawn
    public float radius = 50f; // Radius of the spherical asteroid field
    public float minScale = 0.5f; // Minimum size of asteroids
    public float maxScale = 2f; // Maximum size of asteroids
    public float asteroidRotationSpeed = 20f; // Speed of individual asteroid rotation
    public float fieldRotationSpeed = 10f; // Speed of entire field rotation

    private GameObject[] asteroids; // Array to store spawned asteroids

    void Start()
    {
        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        asteroids = new GameObject[numberOfAsteroids]; // Initialize the array

        for (int i = 0; i < numberOfAsteroids; i++)
        {
            // Generate a random position on a sphere
            Vector3 spawnPosition = Random.onUnitSphere * radius + transform.position;

            // Instantiate the asteroid at the random position
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
            // Set the spawner as the parent so asteroids rotate with the field
            asteroid.transform.SetParent(transform);
            asteroids[i] = asteroid; // Store the asteroid in the array

            // Randomize rotation on all axes for natural appearance
            asteroid.transform.rotation = Random.rotation;

            // Randomize scale for variety
            float randomScale = Random.Range(minScale, maxScale);
            asteroid.transform.localScale = Vector3.one * randomScale;
        }
    }

    void Update()
    {
        RotateAsteroidField();
        RotateIndividualAsteroids();
    }

    void RotateAsteroidField()
    {
        // Rotate the entire spawner (and thus all child asteroids) around the Y axis
        transform.Rotate(0f, fieldRotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    void RotateIndividualAsteroids()
    {
        if (asteroids == null) return; // Safety check

        foreach (GameObject asteroid in asteroids)
        {
            if (asteroid != null)
            {
                // Rotate around each axis with different speeds for a natural tumbling effect
                asteroid.transform.Rotate(
                    Vector3.up * asteroidRotationSpeed * Time.deltaTime,
                    Space.Self
                );
                asteroid.transform.Rotate(
                    Vector3.right * (asteroidRotationSpeed * 0.7f) * Time.deltaTime,
                    Space.Self
                );
                asteroid.transform.Rotate(
                    Vector3.forward * (asteroidRotationSpeed * 0.5f) * Time.deltaTime,
                    Space.Self
                );
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Spaceship destroyed by asteroid collision!");
        }
    }
}