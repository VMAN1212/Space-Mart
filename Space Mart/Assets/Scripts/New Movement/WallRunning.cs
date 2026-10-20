using UnityEngine;
using UnityEngine.InputSystem;

public class WallRunning : MonoBehaviour
{
    [Header("Input (New Input System)")]
    [Tooltip("Assign your Jump action here so you can wall-jump.")]
    public InputActionReference jumpInput;

    [Header("Wall Running")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce = 200f;
    public float wallRunTimer;
    public float maxWallRunTime = 2f;

    [Header("Wall Jumping")]
    public float wallJumpUpForce = 10f;
    public float wallJumpSideForce = 15f;

    [Header("Exiting Wall")]
    private bool exitingWall;
    public float exitWallTime = 0.2f;
    private float exitWallTimer;

    [Header("Detection")]
    public float wallCheckDistance = 0.7f;
    public float minJumpHeight = 1.5f;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform playerCamera;
    public PlayerCam playerCam; // Reference to your camera script for tilting
    private Rigidbody rb;
    private PlayerMovement pm;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;
    }

    private void OnEnable()
    {
        if (jumpInput != null) jumpInput.action.Enable();
    }

    private void OnDisable()
    {
        if (jumpInput != null) jumpInput.action.Disable();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();

        // Check for wall jump input every frame while wall running
        if (pm.state == MovementStateWrapper() && jumpInput != null && jumpInput.action.WasPressedThisFrame())
        {
            WallJump();
        }
    }

    private void FixedUpdate()
    {
        if (pm.state == PlayerMovement.MovementState.wallrunning)
            WallRunningMovement();
    }

    private PlayerMovement.MovementState MovementStateWrapper()
    {
        return PlayerMovement.MovementState.wallrunning;
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, playerCamera.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -playerCamera.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        if (exitingWall)
        {
            pm.state = PlayerMovement.MovementState.air;
            exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0)
                exitingWall = false;
        }
        else if ((wallLeft || wallRight) && AboveGround())
        {
            if (!exitingWall)
            {
                if (pm.state != PlayerMovement.MovementState.wallrunning)
                    StartWallRun();

                if (wallRunTimer > 0)
                {
                    wallRunTimer -= Time.deltaTime;
                }

                if (wallRunTimer <= 0)
                {
                    exitingWall = true;
                    exitWallTimer = exitWallTime;
                    StopWallRun();
                }
            }
        }
        else
        {
            if (pm.state == PlayerMovement.MovementState.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.state = PlayerMovement.MovementState.wallrunning;
        wallRunTimer = maxWallRunTime;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Tilt camera left or right based on the wall side
        if (playerCam != null)
        {
            if (wallRight) playerCam.SetWallTilt(playerCam.wallTilt);
            else if (wallLeft) playerCam.SetWallTilt(-playerCam.wallTilt);
        }
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((playerCamera.forward - wallForward).magnitude > (playerCamera.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        rb.AddForce(-wallNormal * 100f, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.state = PlayerMovement.MovementState.air;
        rb.useGravity = true;

        // Reset camera tilt
        if (playerCam != null)
        {
            playerCam.SetWallTilt(0f);
        }
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 jumpForce = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(jumpForce, ForceMode.Impulse);

        StopWallRun();
    }
}