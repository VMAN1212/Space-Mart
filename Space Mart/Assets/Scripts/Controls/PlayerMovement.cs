using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input (New Input System)")]
    public InputActionReference moveInput;
    public InputActionReference jumpInput;
    public InputActionReference sprintInput;
    public InputActionReference crouchInput;

    [Header("Movement")]
    private float moveSpeed;
    private bool isSprinting;
    public float walkSpeed = 7f;
    public float sprintSpeed = 10f;
    public float slideSpeed = 15f;
    public float groundDrag = 5f;

    [Header("Jumping")]
    public float jumpForce = 6f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    public float gravityMultiplier = 2.5f;
    private bool readyToJump = true;
    public bool canJump = true;

    [Header("Crouching")]
    private bool isCrouching;
    public float crouchSpeed = 3.5f;
    public float crouchYScale = 0.5f;
    private float startYScale;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 40f;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("References")]
    [Tooltip("Drag your Camera transform here.")]
    public Transform playerCamera;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    [HideInInspector] public bool dashigkeit; // or `dashing` bool
    [HideInInspector] public float maxYSpeed;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
        wallrunning,
        dashing
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;
    }

    private void OnEnable()
    {
        moveInput.action.Enable();
        jumpInput.action.Enable();
        sprintInput.action.Enable();
        crouchInput.action.Enable();

        crouchInput.action.performed += OnCrouchToggle;
        sprintInput.action.performed += OnSprintToggle;
    }

    private void OnDisable()
    {
        moveInput.action.Disable();
        jumpInput.action.Disable();
        sprintInput.action.Disable();
        crouchInput.action.Disable();

        crouchInput.action.performed -= OnCrouchToggle;
        sprintInput.action.performed -= OnSprintToggle;
    }

    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f * 3, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // Handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ApplyBetterGravity();
    }

    private void MyInput()
    {
        // Read Vector2 movement
        Vector2 moveValue = moveInput.action.ReadValue<Vector2>();
        horizontalInput = moveValue.x;
        verticalInput = moveValue.y;

        // Continuous jumping check
        if (jumpInput.action.IsPressed() && readyToJump && grounded && canJump)
        {
            readyToJump = false;
            exitingSlope = true;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void OnCrouchToggle(InputAction.CallbackContext context)
    {
        isCrouching = !isCrouching; // Toggle state

        if (isCrouching)
        {
            // Shrink scale
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y - (startYScale - crouchYScale) * 0.5f, transform.position.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else
        {
            // Restore scale
            transform.position = new Vector3(transform.position.x, transform.position.y + (startYScale - crouchYScale) * 0.5f, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void OnSprintToggle(InputAction.CallbackContext context)
    {
        isSprinting = !isSprinting; // Toggle state
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (isCrouching)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // Mode - Sprinting (checking if sprint key is held AND moving forward)
        else if (grounded && isSprinting && verticalInput > 0f)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, whatIsGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0f;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void MovePlayer()
    {
        if (playerCamera == null) return;

        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = forward * verticalInput + right * horizontalInput;

        // Slope movement
        if (OnSlope() && !exitingSlope)
        {
            rb.useGravity = false;
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

            if (rb.linearVelocity.y > 0f)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // Ground movement
        else if (grounded)
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // Air movement
        else
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void ApplyBetterGravity()
    {
        if (!grounded && !OnSlope())
        {
            if (rb.linearVelocity.y < 0f)
            {
                rb.AddForce(Vector3.down * (gravityMultiplier - 1f) * Physics.gravity.magnitude, ForceMode.Acceleration);
            }
        }
    }

    private void SpeedControl()
    {
        // Limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
        // Limiting speed on ground/air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }

        // Inside your velocity limiting function:
        if (maxYSpeed != 0 && rb.linearVelocity.y > maxYSpeed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYSpeed, rb.linearVelocity.z);
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }
}