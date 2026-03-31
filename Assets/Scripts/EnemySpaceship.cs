using UnityEngine;

public class EnemySpaceship : MonoBehaviour
{
    public Transform player;          // Reference to player's transform
    public float moveSpeed = 5f;      // Speed of enemy movement
    public float stoppingDistance = 5f; // Distance to stop from player
    public float raycastDistance = 20f; // Max distance for raycast detection
    public GameObject projectilePrefab; // Bullet prefab to shoot
    public float fireRate = 1f;       // Shots per second
    public Transform firePoint;       // Where projectiles spawn

    public int maxHP = 5;             // Enemy's maximum HP
    private int currentHP;            // Current HP (private, modified on hit)

    private float nextFireTime = 0f;
    private bool obstacleDetected = false;

    void Start()
    {
        player = FindAnyObjectByType<ShipMovement>().transform;
        currentHP = maxHP; // Initialize HP
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference not assigned in " + gameObject.name);
            return;
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Raycast to detect obstacles or player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, raycastDistance))
        {
            obstacleDetected = true;

            // If the raycast hit something, stop and shoot
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            obstacleDetected = false;
        }

        // If no obstacle, move towards the player
        if (!obstacleDetected && distanceToPlayer > stoppingDistance)
        {
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
        }

        transform.LookAt(player); // Always face the player
        Debug.DrawRay(transform.position, directionToPlayer * raycastDistance, Color.red);
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogWarning("Projectile prefab or fire point not assigned!");
        }
    }

    // Called when hit by the player's raycast
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage! HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " destroyed!");
        Destroy(gameObject); // Or play death animation/effects
    }
}