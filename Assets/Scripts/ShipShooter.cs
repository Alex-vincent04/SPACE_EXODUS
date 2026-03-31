using UnityEngine;

public class ShipShooter : MonoBehaviour
{
    // Bullet prefabs
    public GameObject bulletPrefab1;  // Basic bullet
    public GameObject bulletPrefab2;  // Alternative bullet
    public GameObject bulletPrefab3;  // Special bullet

    // Bullet spawn point
    public Transform firePoint;

    // Bullet properties
    public float bulletSpeed = 20f;
    public float bulletLifetime = 2f;  // Time before bullet self-destructs

    // Shooting cooldown
    public float fireRate = 0.2f;
    private float nextFireTime;

    void Update()
    {
        // Shoot different bullets with different keys
        if (Time.time > nextFireTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))  // Basic bullet
            {
                Shoot(bulletPrefab1);
                nextFireTime = Time.time + fireRate;
            }
            else if (Input.GetKeyDown(KeyCode.Z))  // Alternative bullet
            {
                Shoot(bulletPrefab2);
                nextFireTime = Time.time + fireRate;
            }
            else if (Input.GetKeyDown(KeyCode.X))  // Special bullet
            {
                Shoot(bulletPrefab3);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot(GameObject bulletPrefab)
    {
        // Instantiate bullet at fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Add velocity to bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * bulletSpeed;  // Using forward instead of up for 3D

        // Destroy bullet after lifetime
        Destroy(bullet, bulletLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check collision with asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject);  // Destroy spaceship
        }
    }
}