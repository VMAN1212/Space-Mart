using UnityEngine;
using UnityEngine.InputSystem;

public class Dashing : MonoBehaviour
{
    [Header("Input (New Input System)")]
    [Tooltip("Assign your Dash action here (e.g., button bound to 'E').")]
    public InputActionReference dashInput;
    [Tooltip("Assign your Move action here for directional dashing.")]
    public InputActionReference moveInput;

    [Header("References")]
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce = 25f;
    public float dashUpwardForce = 2f;
    public float maxDashYSpeed = 10f;
    public float dashDuration = 0.2f;

    [Header("Camera Effects / FOV")]
    public PlayerCam playerCamScript;
    public float dashFOV = 85f;

    [Header("Cooldown")]
    public float dashCd = 1f;
    private float dashCdTimer;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;

    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        if (playerCam == null && Camera.main != null)
            playerCam = Camera.main.transform;
    }

    private void OnEnable()
    {
        if (dashInput != null) dashInput.action.Enable();
        if (moveInput != null) moveInput.action.Enable();
    }

    private void OnDisable()
    {
        if (dashInput != null) dashInput.action.Disable();
        if (moveInput != null) moveInput.action.Disable();
    }

    private void Update()
    {
        if (moveInput != null)
        {
            Vector2 moveValue = moveInput.action.ReadValue<Vector2>();
            horizontalInput = moveValue.x;
            verticalInput = moveValue.y;
        }

        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }

        if (dashInput != null && dashInput.action.WasPressedThisFrame())
        {
            Dash();
        }
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.dashigkeit = true;
        pm.maxYSpeed = maxDashYSpeed;


        // Use player's transform flattened (ignoring looking up/down) or camera forward
        Transform forwardT = useCameraForward ? playerCam : transform;
        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + Vector3.up * dashUpwardForce;

        StartCoroutine(DelayedForceRoutine(forceToApply));

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        Vector3 flatForward = forwardT.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        Vector3 flatRight = forwardT.right;
        flatRight.y = 0f;
        flatRight.Normalize();

        Vector3 movementDir = Vector3.zero;

        if (allowAllDirections)
        {
            movementDir = flatForward * verticalInput + flatRight * horizontalInput;
        }
        else
        {
            movementDir = flatForward;
        }

        if (horizontalInput == 0 && verticalInput == 0)
        {
            movementDir = flatForward;
        }

        return movementDir.normalized;
    }

    private System.Collections.IEnumerator DelayedForceRoutine(Vector3 forceToApply)
    {
        yield return new WaitForSeconds(0.025f);

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashigkeit = false;
        pm.maxYSpeed = 0f;
    }
}