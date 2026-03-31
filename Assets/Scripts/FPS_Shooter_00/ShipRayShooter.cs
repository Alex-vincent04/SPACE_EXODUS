using UnityEngine;

public class ShipRayShooter : MonoBehaviour
{
    public Transform shootOrigin; // Assign this to the player's hand/head/etc.
    public GameObject hitEffectPrefab;
    public float damage = 10f;
    public float rayDistance = 1000f;
    public float fireRate = 0.2f;

    private float nextFireTime;
    private Ray lastRay; // Stores the last fired ray for debugging

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
        if (shootOrigin == null)
        {
            Debug.LogError("Shoot origin not assigned!");
            return;
        }

        lastRay = new Ray(shootOrigin.position, shootOrigin.forward);
        if (Physics.Raycast(lastRay, out RaycastHit hit, rayDistance))
        {
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }

            EnemySpaceship enemy = hit.collider.GetComponent<EnemySpaceship>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // Deal 1 damage per hit (adjust as needed)
            }
        }
    }

    // Draw Gizmos in the Scene view
    private void OnDrawGizmos()
    {
        if (shootOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(shootOrigin.position, shootOrigin.forward * rayDistance);

        // Draw a small sphere at the origin point
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(shootOrigin.position, 0.05f);

        // If in Play mode, also draw the last fired ray
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(lastRay.origin, lastRay.direction * rayDistance);
        }
    }
}