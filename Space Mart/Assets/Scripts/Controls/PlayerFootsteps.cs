using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;

    [Header("Ground Check")]
    public Transform groundCheck;           // An empty GameObject placed at the player's feet
    public LayerMask groundLayer;           // The layer(s) representing walkable ground
    public float groundCheckRadius = 0.3f;  // Size of the overlap sphere

    [Header("State Thresholds & Intervals")]
    public float crouchStepInterval = 0.65f;
    public float walkStepInterval = 0.38f;
    public float sprintStepInterval = 0.26f;

    [Header("Speed Thresholds")]
    public float minSpeedToPlay = 1.5f;
    public float sprintSpeedThreshold = 11.0f;

    private float stepTimer;

    private void Update()
    {
        if (rb == null) return;

        // 1. CRITICAL: If the player is in the air, stop footstep logic entirely
        if (!IsGrounded())
        {
            stepTimer = 0f; // Reset timer so a footstep triggers immediately upon landing
            return;
        }

        // Calculate current horizontal movement speed
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

        // Check if moving fast enough to trigger footsteps
        if (horizontalSpeed > minSpeedToPlay)
        {
            float currentInterval = walkStepInterval;

            if (horizontalSpeed >= sprintSpeedThreshold)
            {
                currentInterval = sprintStepInterval;
            }
            else if (horizontalSpeed <= 5.0f)
            {
                currentInterval = crouchStepInterval;
            }

            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = currentInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return true; // Fallback if not assigned yet

        // Checks if a small sphere at the player's feet overlaps with the ground layer
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void PlayFootstepSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound2D("Footsteps");
        }
    }
}
