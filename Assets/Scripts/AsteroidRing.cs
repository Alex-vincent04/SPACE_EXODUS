using UnityEngine;

public class AsteroidRing : MonoBehaviour
{
    public float G = 1f; // Gravitational constant (match with CelestialRotation)
    public GameObject planet; // The planet the asteroids orbit
    public GameObject asteroidPrefab; // Prefab for asteroid GameObjects
    public int asteroidCount = 50; // Number of asteroids in the ring
    public float ringRadius = 5f; // Radius of the asteroid ring
    public float ringThickness = 0.5f; // Thickness of the ring (random offset)
    public float asteroidMass = 100f; // Fixed mass for asteroids

    private GameObject[] asteroids; // Array to store spawned asteroids
    private Rigidbody planetRb;

    void Start()
    {
        // Safety check for planet
        if (planet == null)
        {
            planet = GameObject.FindGameObjectWithTag("Celestial");
            if (planet == null)
            {
                Debug.LogError("No planet assigned or found with tag 'Celestial'!");
                return;
            }
        }

        planetRb = planet.GetComponent<Rigidbody>();
        if (planetRb == null)
        {
            Debug.LogError("Planet (" + planet.name + ") is missing a Rigidbody!");
            return;
        }

        // Safety check for asteroid prefab
        if (asteroidPrefab == null)
        {
            Debug.LogError("Asteroid prefab not assigned!");
            return;
        }

        // Spawn and initialize the asteroid ring
        SpawnAsteroidRing();
    }

    private void FixedUpdate()
    {
        if (asteroids == null || planet == null) return;
        ApplyGravityToAsteroids();
    }

    void SpawnAsteroidRing()
    {
        asteroids = new GameObject[asteroidCount];

        for (int i = 0; i < asteroidCount; i++)
        {
            // Calculate angle for even distribution around the ring
            float angle = i * (360f / asteroidCount) * Mathf.Deg2Rad;
            Vector3 basePosition = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * ringRadius;

            // Add random offset for thickness
            Vector3 offset = new Vector3(
                Random.Range(-ringThickness, ringThickness),
                Random.Range(-ringThickness, ringThickness),
                Random.Range(-ringThickness, ringThickness)
            );

            // Position relative to planet
            Vector3 spawnPosition = planet.transform.position + basePosition + offset;

            // Instantiate asteroid
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
            asteroids[i] = asteroid;

            // Configure Rigidbody
            Rigidbody rb = asteroid.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = asteroid.AddComponent<Rigidbody>();
            }
            rb.mass = asteroidMass;
            rb.useGravity = false; // Disable Unity's built-in gravity
            rb.constraints = RigidbodyConstraints.None; // Ensure no constraints

            // Set initial orbital velocity
            SetAsteroidVelocity(asteroid);
        }
    }

    void SetAsteroidVelocity(GameObject asteroid)
    {
        Rigidbody asteroidRb = asteroid.GetComponent<Rigidbody>();
        float planetMass = planetRb.mass;
        float r = Vector3.Distance(asteroid.transform.position, planet.transform.position);

        // Direction from asteroid to planet
        Vector3 directionToPlanet = (planet.transform.position - asteroid.transform.position).normalized;

        // Orbital velocity direction (perpendicular to radius in the XZ plane)
        Vector3 velocityDirection = Vector3.Cross(directionToPlanet, Vector3.up).normalized;
        float velocityMagnitude = Mathf.Sqrt((G * planetMass) / r);

        // Include the planet's velocity so asteroids move with it
        Vector3 planetVelocity = planetRb.linearVelocity;
        asteroidRb.linearVelocity = planetVelocity + (velocityDirection * velocityMagnitude);

        // Debug log to verify setup
        Debug.Log($"Asteroid {asteroid.name}: Distance={r}, Orbit Velocity={velocityMagnitude}, Planet Velocity={planetVelocity}, Total Velocity={asteroidRb.linearVelocity}");
    }

    void ApplyGravityToAsteroids()
    {
        float planetMass = planetRb.mass;

        foreach (GameObject asteroid in asteroids)
        {
            if (asteroid == null) continue;

            Rigidbody asteroidRb = asteroid.GetComponent<Rigidbody>();
            float r = Vector3.Distance(asteroid.transform.position, planet.transform.position);

            Vector3 direction = (planet.transform.position - asteroid.transform.position).normalized;
            float forceMagnitude = G * (asteroidMass * planetMass) / (r * r);

            // Apply gravitational force to keep asteroids orbiting
            asteroidRb.AddForce(direction * forceMagnitude);

            // Debug log to check force application
            Debug.Log($"Asteroid {asteroid.name}: Force={forceMagnitude}, Position={asteroid.transform.position}");
        }
    }
}