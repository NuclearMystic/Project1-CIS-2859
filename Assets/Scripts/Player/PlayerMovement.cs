using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;

    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float acceleration = 10f;  // Speeding up rate
    public float deceleration = 15f;  // Slowing down rate
    public float airControl = 0.5f;   // How much control you have in the air
    public float gravity = -9.8f;     // Gravity force
    public float jumpHeight = 2f;     // How high the player can jump

    private Vector3 velocity;
    private Vector3 moveDirection;
    private float currentSpeed;
    private float targetSpeed;

    private bool isRunning;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentSpeed = 0f;
    }

    void Update()
    {
        HandleMovement();
        ApplyGravity();
        HandleJumping();
        characterController.Move((moveDirection * currentSpeed + velocity) * Time.deltaTime);
    }

    private void HandleMovement()
    {
        // Check if grounded
        isGrounded = characterController.isGrounded;

        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determine target speed
        targetSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;

        // Calculate desired movement direction
        Vector3 desiredDirection = transform.right * x + transform.forward * z;

        // Gradually adjust currentSpeed for smooth acceleration/deceleration
        if (desiredDirection.magnitude > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Apply movement
        if (isGrounded)
        {
            moveDirection = desiredDirection.normalized;
        }
        else
        {
            // Allow some air control
            moveDirection = Vector3.Lerp(moveDirection, desiredDirection.normalized, airControl * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps the character grounded
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
