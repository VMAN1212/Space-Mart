using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip[] footstepClips;

    [Header("Timing & Speed")]
    public float stepInterval = 0.4f; // Time between steps (lower = faster footsteps)
    public float minSpeedToPlay = 2f;  // Minimum horizontal speed required to trigger footsteps
    private float stepTimer;

    private void Update()
    {
        if (rb == null || audioSource == null || footstepClips.Length == 0) return;

        // Calculate horizontal speed (ignoring vertical jumping/falling)
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

        // Check if the player is moving fast enough
        if (horizontalSpeed > minSpeedToPlay)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayRandomFootstep();
                stepTimer = stepInterval; // Reset the timer
            }
        }
        else
        {
            // Reset timer immediately so the first step plays instantly when starting to walk
            stepTimer = 0f;
        }
    }

    private void PlayRandomFootstep()
    {
        // Pick a random clip from your array to avoid a repetitive looping sound
        int randomIndex = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[randomIndex];

        // Play the sound once without interrupting other sounds
        audioSource.PlayOneShot(clip);
    }
}