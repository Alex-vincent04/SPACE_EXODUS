using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    // Movement settings
    public float Speed = 50f;
    public float TurnSpeed = 60f;
    public float BoostMultiplier = 2f; // How much boost increases speed
    public float MouseSensitivity = 2f; // How sensitive mouse movements are

    private float currentSpeed; // Actual speed being used (normal or boosted)
    private bool isBoosting = false;

    void Start()
    {
        currentSpeed = Speed; // Initialize with normal speed

        // Load last position if available
        if (PlanetLoader.LastShipPosition != Vector3.zero)
        {
            transform.position = PlanetLoader.LastShipPosition;
        }
    }

    void Update()
    {
        HandleBoost();
        Turn();
        Thrust();
    }

    void HandleBoost()
    {
        // Check for boost input
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isBoosting = true;
            currentSpeed = Speed * BoostMultiplier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBoosting = false;
            currentSpeed = Speed;
        }
    }

    void Turn()
    {
        // Keyboard turning
        float yaw = TurnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float pitch = TurnSpeed * Time.deltaTime * Input.GetAxis("Pitch");
        float roll = TurnSpeed * Time.deltaTime * Input.GetAxis("Roll");

        // Mouse turning (adds to keyboard input)
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * MouseSensitivity; // Inverted for natural feel

        // Combine inputs
        transform.Rotate(pitch + mouseY, yaw + mouseX, roll);
    }

    void Thrust()
    {
        // Forward movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }

        // Strafe movements (up/down/left/right)
        float strafeHorizontal = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float strafeVertical = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        transform.position += transform.right * strafeHorizontal;
        transform.position += transform.up * strafeVertical;
    }
}