using UnityEngine;

public class Bullet3D : MonoBehaviour
{
    public float lifetime = 2f;
    public float damage = 1f;
    public float speed = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider collision)
    {
        // Check for asteroid collision
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Asteroid destroyed");
            Destroy(gameObject);
        }
        // Check for enemy collision
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Enemy destroyed");
            Destroy(gameObject);
        }
    }
}