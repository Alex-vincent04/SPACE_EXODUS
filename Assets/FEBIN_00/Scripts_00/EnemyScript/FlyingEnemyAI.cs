using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    // public Transform player; // Assign player GameObject in the Inspector
    public GameObject player;
    public float detectionRange = 15f; // Range to detect the player
    public float moveSpeed = 5f; // Speed of movement
    public float rotationSpeed = 3f; // Speed of rotation
    public float hoverStrength = 1f; // How much the enemy bobs up and down
    public float hoverSpeed = 2f; // Speed of the hovering effect

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        
    }

    private void Update()
    {
        Vector3 playerPosition = player.transform.position; // Use player GameObject's transform

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Move towards the player while maintaining altitude
            Vector3 targetPosition = new Vector3(player.transform.position.x, startPosition.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Rotate to face the player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // Add hovering effect
        transform.position += new Vector3(0, Mathf.Sin(Time.time * hoverSpeed) * hoverStrength * Time.deltaTime, 0);
    }
}
