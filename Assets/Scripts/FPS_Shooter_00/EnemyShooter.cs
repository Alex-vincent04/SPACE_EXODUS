using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform player;              // Reference to player
    public GameObject primaryBulletPrefab; // Regular bullet prefab
    public GameObject specialBulletPrefab; // Sure-hit bullet prefab
    public Transform firePoint;           // Where bullets spawn
    public float primaryFireRate = 1f;    // Shots per second for primary
    public float specialFireRate = 10f;   // Seconds between special shots
    public float primaryBulletSpeed = 20f;
    public float specialBulletSpeed = 15f;
    public float maxAttackRange = 15f;    // Max range to attack the player

    private float primaryFireTimer = 0f;
    private float specialFireTimer = 0f;
    private bool canShoot = true;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        specialFireTimer = specialFireRate; // Start ready to use special
    }

    void Update()
    {
        // Check attack range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > maxAttackRange) return; // Don't shoot if out of range

        // Look at player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Fire timing
        primaryFireTimer += Time.deltaTime;
        specialFireTimer += Time.deltaTime;

        if (canShoot)
        {
            // Primary shot
            if (primaryFireTimer >= 1f / primaryFireRate)
            {
                ShootPrimary();
                primaryFireTimer = 0f;
            }

            // Special shot
            if (specialFireTimer >= specialFireRate)
            {
                ShootSpecial();
                specialFireTimer = 0f;
            }
        }
    }

    void ShootPrimary()
    {
        GameObject bullet = Instantiate(primaryBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Ignore collision with the enemy
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

        rb.linearVelocity = firePoint.forward * primaryBulletSpeed;
        Destroy(bullet, 5f);
    }

    void ShootSpecial()
    {
        GameObject bullet = Instantiate(specialBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Ignore collision with the enemy
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

        StartCoroutine(HomingBullet(bullet));
        Destroy(bullet, 5f);
    }

    System.Collections.IEnumerator HomingBullet(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        float lifetime = 0f;

        while (bullet != null && lifetime < 5f)
        {
            Vector3 direction = (player.position - bullet.transform.position).normalized;
            rb.linearVelocity = direction * specialBulletSpeed;
            bullet.transform.rotation = Quaternion.LookRotation(direction);

            lifetime += Time.deltaTime;
            yield return null;
        }
    }
}
