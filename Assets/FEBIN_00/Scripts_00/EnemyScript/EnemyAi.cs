using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Assign the player GameObject in the Inspector
    public float detectionRange = 10f; // Distance at which enemy detects player
    public float moveSpeed = 3f; // Speed of enemy movement
    public float rotationSpeed = 5f; // Speed of enemy rotation

    private void Update()
    {
        if (player == null) return; // Prevent errors if no player is assigned

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Rotate to face the player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Keep enemy upright
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Move toward the player
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
