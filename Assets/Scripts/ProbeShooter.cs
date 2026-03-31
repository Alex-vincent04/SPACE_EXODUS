using UnityEngine;
using UnityEngine.SceneManagement;

public class ProbeShooter : MonoBehaviour
{
    public GameObject Probe;
    public Transform firePoint;         // Where the probe spawns from
    public Transform returnTarget;      // The moving object to return to
    public float probeSpeed = 10f;      // Speed of the probe movement
    public bool hasProbe = true;

    private GameObject activeProbe;     // To keep track of the instantiated probe
    private bool isReturning = false;   // To track if probe is returning

    void Start()
    {
        // Make sure returnTarget is assigned in the Inspector
        if (returnTarget == null)
        {
            Debug.LogError("ReturnTarget not assigned in ProbeShooter!");
        }
    }

    void Launch()
    {
        activeProbe = Instantiate(Probe, firePoint.position, firePoint.rotation);
        hasProbe = false;
        isReturning = false;
    }

    void Update()
    {
        // Launch probe with Left Alt
        if (Input.GetKeyDown(KeyCode.LeftAlt) && hasProbe)
        {
            Launch();
        }

        // Control probe movement
        if (activeProbe != null && !isReturning)
        {
            // Move probe forward in its local forward direction
            activeProbe.transform.Translate(Vector3.forward * probeSpeed * Time.deltaTime);
        }

        // Recall probe with R key
        if (Input.GetKeyDown(KeyCode.H) && !hasProbe)
        {
            CallBack();
        }

        // Handle probe return movement
        if (activeProbe != null && isReturning && returnTarget != null)
        {
            // Move probe back to the moving returnTarget's current position
            activeProbe.transform.position = Vector3.MoveTowards(
                activeProbe.transform.position,
                returnTarget.position,
                probeSpeed * Time.deltaTime
            );

            // Check if probe has reached returnTarget's position
            if (Vector3.Distance(activeProbe.transform.position, returnTarget.position) < 0.1f)
            {
                Destroy(activeProbe);
                hasProbe = true;
                isReturning = false;
            }
        }
    }

    void CallBack()
    {
        if (activeProbe != null)
        {
            isReturning = true;
        }
    }
}