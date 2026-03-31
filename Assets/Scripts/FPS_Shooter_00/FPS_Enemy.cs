using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float enemySpeed = 4f;
    public float rotateSpeed = 10f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public GameObject deathPrefab;
    public float obstacleDetectionDistance = 5f;
    public float heightOffset = 1.0f;
    public float enemyHealth = 20f;

    private Rigidbody rb;
    private Vector3 avoidanceDirection;
    private float avoidanceTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();

        if (rb)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (player == null) return;

        LookAtPlayer();
        EnemyFollow();
        GroundEnemy();
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
    }

    void EnemyFollow()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 10f)
        {
            Vector3 moveDirection = (player.transform.position - transform.position).normalized;

            // Obstacle avoidance
            if (avoidanceTimer > 0)
            {
                moveDirection = avoidanceDirection;
                avoidanceTimer -= Time.deltaTime;
            }
            else if (Physics.Raycast(transform.position, transform.forward, obstacleDetectionDistance, obstacleLayer))
            {
                // Try to find a new direction
                if (!Physics.Raycast(transform.position, transform.right, obstacleDetectionDistance, obstacleLayer))
                {
                    avoidanceDirection = transform.right;
                }
                else if (!Physics.Raycast(transform.position, -transform.right, obstacleDetectionDistance, obstacleLayer))
                {
                    avoidanceDirection = -transform.right;
                }
                else
                {
                    avoidanceDirection = -transform.forward;
                }
                avoidanceTimer = 1f;
                moveDirection = avoidanceDirection;
            }

            transform.position += moveDirection * enemySpeed * Time.deltaTime;
        }
    }

    void GroundEnemy()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * heightOffset;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 10f, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + heightOffset, transform.position.z);
        }
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1f);
            Destroy(other.gameObject);
        }
    }

    void Die()
    {
        if (deathPrefab != null)
        {
            Instantiate(deathPrefab, transform.position + transform.forward * 2, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}