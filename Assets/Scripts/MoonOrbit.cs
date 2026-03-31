using UnityEngine;

public class MoonOrbit : MonoBehaviour
{
    public float G = 1000f; // Gravitational constant
    public GameObject[] celestials; // Array of celestial bodies
    public GameObject moon; // The moon object
    private GameObject nearestCelestial; // The planet the moon orbits
    private Rigidbody moonRb;
    private Rigidbody planetRb;

    void Start()
    {
        // Find all celestial objects and the moon
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        moon = GameObject.FindGameObjectWithTag("Moon");

        // Safety checks
        if (celestials.Length == 0)
        {
            Debug.LogError("No objects with tag 'Celestial' found!");
            return;
        }
        if (moon == null)
        {
            Debug.LogError("No object with tag 'Moon' found!");
            return;
        }

        moonRb = moon.GetComponent<Rigidbody>();
        if (moonRb == null)
        {
            Debug.LogError("Moon is missing a Rigidbody component!");
            return;
        }

        // Find the nearest celestial and set initial velocity
        FindNearestCelestial();
        if (nearestCelestial != null)
        {
            planetRb = nearestCelestial.GetComponent<Rigidbody>();
            if (planetRb == null)
            {
                Debug.LogError("Nearest celestial (" + nearestCelestial.name + ") is missing a Rigidbody!");
                return;
            }
            SetInitialVelocity();
        }
    }

    private void FixedUpdate()
    {
        if (nearestCelestial != null && moon != null)
        {
            ApplyGravity();
        }
    }

    void FindNearestCelestial()
    {
        float minDistance = float.MaxValue;
        nearestCelestial = null;

        foreach (GameObject celestial in celestials)
        {
            float distance = Vector3.Distance(moon.transform.position, celestial.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCelestial = celestial;
            }
        }

        if (nearestCelestial == null)
        {
            Debug.LogError("No nearest celestial found!");
        }
        else
        {
            Debug.Log("Moon will orbit: " + nearestCelestial.name);
        }
    }

    void ApplyGravity()
    {
        // Calculate gravitational force between moon and planet
        float m1 = moonRb.mass;
        float m2 = planetRb.mass;
        float r = Vector3.Distance(moon.transform.position, nearestCelestial.transform.position);

        Vector3 direction = (nearestCelestial.transform.position - moon.transform.position).normalized;
        float forceMagnitude = G * (m1 * m2) / (r * r);

        // Apply force to the moon
        moonRb.AddForce(direction * forceMagnitude);

        // Optional: Apply opposite force to planet (Newton's 3rd law), though usually negligible
        // planetRb.AddForce(-direction * forceMagnitude);
    }

    void SetInitialVelocity()
    {
        // Calculate distance and mass
        float m2 = planetRb.mass;
        float r = Vector3.Distance(moon.transform.position, nearestCelestial.transform.position);

        // Orient the moon towards the planet
        moon.transform.LookAt(nearestCelestial.transform);

        // Set initial orbital velocity relative to planet's velocity
        Vector3 velocityDirection = moon.transform.right; // Perpendicular to direction towards planet
        float velocityMagnitude = Mathf.Sqrt((G * m2) / r); // Circular orbit velocity

        // Adjust for planet's velocity to keep orbit stable
        Vector3 planetVelocity = planetRb.linearVelocity;
        moonRb.linearVelocity = planetVelocity + (velocityDirection * velocityMagnitude);
    }
}