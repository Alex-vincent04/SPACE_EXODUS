using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    private Vector3 targetDirection; // Direction to move toward
    private bool hasTarget = false;

    // Option 1: Pass player Transform when spawning (preferred method)
    public void SetTarget(Transform playerTransform)
    {
        if (playerTransform != null)
        {
            targetDirection = (playerTransform.position - transform.position).normalized;
            hasTarget = true;
        }
    }

    void Start()
    {
        // Option 2: Find player by tag if no target set
        if (!hasTarget)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetDirection = (player.transform.position - transform.position).normalized;
                hasTarget = true;
            }
        }

        // Destroy projectile after lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (hasTarget)
        {
            // Move in the direction of the target
            transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider collision)
{
    Transform parentObject = collision.transform.parent; // Get the parent of the collided object

    if (collision.gameObject.CompareTag("Player"))
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            print("Hit Player");
            playerHealth.TakeDamage(1f); // Reduce player health
        }

        Destroy(gameObject); // Destroy projectile
    }

    if (parentObject != null)
    {
        print("Parent Object: " + parentObject.name);
    }
}

}
