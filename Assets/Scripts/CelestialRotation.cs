using UnityEngine;

public class CelestialRotation : MonoBehaviour
{
    public float G = 1000f; // Gravitational constant
    public GameObject[] celestials; // Array of celestial bodies
    public GameObject sun; // Explicit reference to the sun (central body)

    // Start is called once before the first execution of Update
    void Start()
    {
        // Find all objects tagged as "Celestial" (fix the tag in Unity if needed)
        celestials = GameObject.FindGameObjectsWithTag("Celestial");

        // Safety check: Ensure there are celestial bodies
        if (celestials.Length == 0)
        {
            Debug.LogError("No objects with tag 'Celestial' found!");
            return;
        }

        // If sun is not assigned in the Inspector, default to the first celestial
        if (sun == null && celestials.Length > 0)
        {
            sun = celestials[0];
            Debug.LogWarning("Sun not assigned in Inspector. Defaulting to: " + sun.name);
        }

        InitialVelocity();
    }

    // Update is called once per frame (empty for now)
    void Update()
    {
        // Add any per-frame logic here if needed
    }

    // FixedUpdate for physics calculations
    private void FixedUpdate()
    {
        Gravity();
    }

    void Gravity()
    {
        foreach (GameObject a in celestials)
        {
            foreach (GameObject b in celestials)
            {
                if (!a.Equals(b)) // Skip self-interaction
                {
                    Rigidbody rbA = a.GetComponent<Rigidbody>();
                    Rigidbody rbB = b.GetComponent<Rigidbody>();

                    // Safety check for Rigidbody components
                    if (rbA == null || rbB == null)
                    {
                        Debug.LogError("Rigidbody missing on " + (rbA == null ? a.name : b.name));
                        continue;
                    }

                    float m1 = rbA.mass;
                    float m2 = rbB.mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    // Apply gravitational force (Newton's law of gravitation)
                    Vector3 direction = (b.transform.position - a.transform.position).normalized;
                    float forceMagnitude = G * (m1 * m2) / (r * r);
                    rbA.AddForce(direction * forceMagnitude);
                }
            }
        }
    }

    void InitialVelocity()
    {
        foreach (GameObject a in celestials)
        {
            foreach (GameObject b in celestials)
            {
                if (a != b) // Skip self-interaction
                {
                    Rigidbody rbA = a.GetComponent<Rigidbody>();
                    Rigidbody rbB = b.GetComponent<Rigidbody>();

                    // Safety check for Rigidbody components
                    if (rbA == null || rbB == null)
                    {
                        Debug.LogError("Rigidbody missing on " + (rbA == null ? a.name : b.name));
                        continue;
                    }

                    float m2 = rbB.mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    // Orient the object towards the other body
                    a.transform.LookAt(b.transform);

                    // Calculate initial orbital velocity (simplified circular orbit)
                    Vector3 velocityDirection = a.transform.right; // Perpendicular to look direction
                    float velocityMagnitude = Mathf.Sqrt((G * m2) / r);
                    rbA.linearVelocity += velocityDirection * velocityMagnitude;
                }
            }
        }
    }
}