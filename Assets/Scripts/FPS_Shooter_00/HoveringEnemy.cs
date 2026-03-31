using UnityEngine;

public class HoveringEnemy : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject player;
    public float detectionRange = 15f;

    [Header("Movement Settings")]
    public float hoverHeight = 5f;
    public float movementSpeed = 3f;
    public float rotationSpeed = 5f;
    public float verticalOscillationSpeed = 1f;
    public float verticalOscillationAmount = 0.5f;

    [Header("Attack Settings")]
    public float attackCooldown = 3f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    [Header("Health Settings")]
    public float enemyHealth = 15f;
    public GameObject deathEffect;

    private float nextAttackTime;
    private float baseHeight;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.freezeRotation = true;
        }

        baseHeight = hoverHeight;

        // Find player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        if (player == null) return;

        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Hovering movement
        HoverMovement();

        // Only engage if player is in range
        if (distanceToPlayer <= detectionRange)
        {
            FacePlayer();
            ApproachPlayer(distanceToPlayer);

            // Attack if ready
            if (Time.time >= nextAttackTime && distanceToPlayer < detectionRange * 0.7f)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void HoverMovement()
    {
        // Vertical oscillation for hovering effect
        float newHeight = baseHeight + Mathf.Sin(Time.time * verticalOscillationSpeed) * verticalOscillationAmount;

        // Maintain hover height
        Vector3 newPosition = new Vector3(
            transform.position.x,
            newHeight,
            transform.position.z
        );

        transform.position = newPosition;
    }

    void FacePlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // Keep the enemy level

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void ApproachPlayer(float currentDistance)
    {
        // Move toward player but maintain hover height
        Vector3 targetPosition = player.transform.position;
        targetPosition.y = transform.position.y; // Keep our current hover height

        float approachSpeed = Mathf.Lerp(0, movementSpeed, currentDistance / detectionRange);
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            approachSpeed * Time.deltaTime
        );
    }

    void Attack()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Create projectile
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        // Set projectile velocity
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            Vector3 attackDirection = (player.transform.position - firePoint.position).normalized;
            projectileRb.linearVelocity = attackDirection * projectileSpeed;
        }

        // Destroy projectile after some time
        Destroy(projectile, 5f);
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1f);
            Destroy(other.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw hover height
        Gizmos.color = Color.cyan;
        Vector3 hoverLineStart = transform.position;
        hoverLineStart.y = 0;
        Vector3 hoverLineEnd = hoverLineStart;
        hoverLineEnd.y = hoverHeight;
        Gizmos.DrawLine(hoverLineStart, hoverLineEnd);
    }
}