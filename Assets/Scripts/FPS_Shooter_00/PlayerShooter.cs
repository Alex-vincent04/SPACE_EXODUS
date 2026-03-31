using UnityEngine;
using System.Collections;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;  // Bullet prefab
    public Transform firePoint;      // Bullet spawn position
    public float bulletSpeed = 20f;
    public int maxAmmo = 10;         // Max bullets before reload
    public float reloadTime = 2f;    // Time to reload
    public float fireRate = 0.2f;    // Time between shots

    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;  // Initialize ammo
    }

    void Update()
    {
        if (isReloading) return;  // Prevent shooting while reloading

        // Left Mouse Click to Shoot
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload()); // Auto reload if no ammo
            }
        }

        // Right Mouse Click to Reload (only if ammo is not full)
        if (Input.GetButtonDown("Fire2") && currentAmmo < maxAmmo && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireRate; // Set fire cooldown
        currentAmmo--;  // Decrease ammo

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.linearVelocity = firePoint.forward * bulletSpeed; // Corrected bullet movement

        Destroy(bullet, 5f);  // Destroy bullet after 5 seconds
    }

    IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime); // Wait for reload time

        currentAmmo = maxAmmo; // Refill ammo
        isReloading = false;
    }
}
