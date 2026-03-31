using UnityEngine;

public class Player_MovemenT : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float jumpForce = 5f;
    public float playerHealth = 30f;

    public Animator animator;
    private Rigidbody rb;
    private bool isGrounded;

    public Transform cameraTransform;
    private float verticalRotation = 0f;
    public float verticalRotationLimit = 80f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize camera if not set
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }
    }

    void Update()
    {
        Move();
        Rotate();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Move()
    {
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveZ = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveZ = -1f;
        }

        Vector3 moveDirection = transform.forward * moveZ * speed * Time.deltaTime;
        transform.position += moveDirection;

        // Animator updates
        if (animator != null)
        {
            animator.SetBool("isMovingForward", moveZ > 0);
            animator.SetBool("isMovingBackward", moveZ < 0);
        }
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Horizontal rotation (Yaw)
        transform.Rotate(0, mouseX, 0);

        // Vertical rotation (Pitch) applied to the camera
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedOrb"))
        {
            HandleRedOrbPickup(other.gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            HandleBulletHit(other.gameObject);
        }
    }

    private void HandleRedOrbPickup(GameObject orb)
    {
        // Safe orb pickup handling
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem("RedOrb");
        }

        var uiManager = FindAnyObjectByType<OrbUIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateOrbCount();
        }

        Destroy(orb);
    }

    private void HandleBulletHit(GameObject bullet)
    {
        playerHealth -= 1;
        Destroy(bullet);

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}