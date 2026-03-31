using UnityEngine;

public class RaycastShooter : MonoBehaviour
{
    public Camera cam;
    public GameObject hitEffectPrefab;
    public float damage = 10f;
    public float rayDistance = 1000f;
    public float fireRate = 0.2f;

    public float Xval = 0.5f; // Clamped between 0 and 1
    public float Yval = 0.5f; // Clamped between 0 and 1

    private float nextFireTime;
    private Ray lastRay; // Stores the last fired ray for debugging

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            ShootRay();
            nextFireTime = Time.time + fireRate;
        }
    }

void ShootRay()
{
    lastRay = cam.ViewportPointToRay(new Vector3(Xval, Yval));
    if (Physics.Raycast(lastRay, out RaycastHit hit, rayDistance))
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(effect, 2f); // Corrected to destroy the instantiated effect, not the prefab
        }

        EnemySpaceship enemySpaceship = hit.collider.GetComponent<EnemySpaceship>();
        if (enemySpaceship != null)
        {
            enemySpaceship.TakeDamage(1);
        }

        Enemy enemyShooter = hit.collider.GetComponent<Enemy>();
        if (enemyShooter != null)
        {
            enemyShooter.TakeDamage(1); // Assuming EnemyShooter has a TakeDamage method
        }
    }
}

    // Draw Gizmos in the Scene view
    private void OnDrawGizmos()
    {
        if (cam == null) return;

        Gizmos.color = Color.red;
        Ray previewRay = cam.ViewportPointToRay(new Vector3(Xval, Yval));
        Gizmos.DrawRay(previewRay.origin, previewRay.direction * rayDistance);

        // Draw a small sphere at the ray origin
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(previewRay.origin, 0.05f);

        // If in Play mode, also draw the last fired ray
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(lastRay.origin, lastRay.direction * rayDistance);
        }
    }
}