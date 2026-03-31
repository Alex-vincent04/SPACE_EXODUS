using UnityEngine;

public class FPS_Enemy : MonoBehaviour
{
    public GameObject player;
    public float enemySpeed = 4f;
    public float rotateSpeed = 10f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public GameObject Prefab;
    public float obstacleDetectionDistance = 5f;

    private Rigidbody rb;
    public float enemyHealth = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure Rigidbody is Kinematic to avoid unwanted physics interactions
        if (rb)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        LookAtPlayer();
        EnemyFollow();
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    }

    void EnemyFollow()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        bool obstacleFront = Physics.Raycast(transform.position, transform.forward, obstacleDetectionDistance, obstacleLayer);
        bool obstacleRight = Physics.Raycast(transform.position, transform.right, obstacleDetectionDistance, obstacleLayer);
        bool obstacleLeft = Physics.Raycast(transform.position, -transform.right, obstacleDetectionDistance, obstacleLayer);

        if (Vector3.Distance(transform.position, player.transform.position) > 5f)
        {
            if (!obstacleFront)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemySpeed * Time.deltaTime);
            }
            else if (!obstacleRight)
            {
                transform.position += transform.right * enemySpeed * Time.deltaTime;
            }
            else if (!obstacleLeft)
            {
                transform.position -= transform.right * enemySpeed * Time.deltaTime;
            }
        }

        GroundEnemy();
    }

    void GroundEnemy()
    {
        RaycastHit hit;
        float enemyHeight = transform.localScale.y;
        Vector3 rayOrigin = transform.position + Vector3.up;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 10f, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + (enemyHeight / 2f), transform.position.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            enemyHealth -= 1;
            Destroy(other.gameObject); // Destroy bullet

            if (enemyHealth <= 0) // Immediately check for death
            {
                Die();
            }
        }
    }

    void Die()
    {
        Instantiate(Prefab, transform.position + transform.forward * 2, Quaternion.identity);
        Destroy(gameObject);
    }
}
