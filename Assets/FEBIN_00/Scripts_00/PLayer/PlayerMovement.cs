using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera; // Reference to the camera (assign in Inspector)
    public PlayerHealth playerHealth; // Reference to PlayerHealth component
    public ParticleSystem hitParticlePrefab; // Particle system prefab for damage effect

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    public float mouseSensitivity = 2f; // Sensitivity for mouse look
    public float maxLookAngle = 80f;    // Maximum angle for looking up/down

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public int TotalOrbs;

    Vector3 velocity;
    bool isGrounded;
    float verticalLookRotation; // Tracks the vertical camera angle

    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the PlayerHealth component
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found on this GameObject!");
        }
    }

    void Update()
    {
        if (playerHealth == null) return;

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Mouse look input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player body horizontally (left/right) only
        transform.Rotate(0f, mouseX, 0f); // Explicitly rotate only around Y-axis

        // Rotate camera vertically (up/down) independently
        verticalLookRotation -= mouseY; // Subtract mouseY to invert (up moves camera up)
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);

        // Jump logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedOrb"))
        {
            InventoryManager.Instance.AddItem("RedOrb");
            FindAnyObjectByType<OrbUIManager>().UpdateOrbCount(); // Update UI
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            HandleBulletHit(other.gameObject);
        }
    }

    private void HandleBulletHit(GameObject bullet)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1f); // Apply 1 damage through PlayerHealth
            Debug.Log($"Player took 1 damage. Current HP: {playerHealth.currentHealth}");

            // Instantiate particle system at player's position if prefab is assigned
            if (hitParticlePrefab != null)
            {
                ParticleSystem hitEffect = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
                Destroy(hitEffect.gameObject, 1f); // Destroy after 1 second
            }
        }
        Destroy(bullet);
    }
}