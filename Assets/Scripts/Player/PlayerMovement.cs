using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public float airControl = 0.5f;

    [Header("Jump & Gravity Settings")]
    public float gravity = -9.8f;
    public float jumpHeight = 2f;

    [Header("Ground Check Settings")]
    public Transform groundCheck; 
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    private Vector3 velocity;
    private Vector3 moveDirection;
    private float currentSpeed;
    private float targetSpeed;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentSpeed = 0f;
    }

    void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        ApplyGravity();
        HandleJumping();
        characterController.Move((moveDirection * currentSpeed + velocity) * Time.deltaTime);
    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 desiredDirection = transform.right * x + transform.forward * z;

        if (desiredDirection.magnitude > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        if (isGrounded)
        {
            moveDirection = desiredDirection.normalized;
        }
        else
        {
            moveDirection = Vector3.Lerp(moveDirection, desiredDirection.normalized, airControl * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensures the player sticks to the ground when landing
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
