using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform Player;

    public Transform player;
    public float heightOffset = 20f;
    public float rotationSpeed = 5f; // Controls how fast it catches up

    private void LateUpdate()
    {
        if (player == null) return;

        // 1. Follow position (including jumping/height changes)
        Vector3 newPosition = player.position;
        newPosition.y += heightOffset;
        transform.position = newPosition;

        // 2. Smoothly rotate to match the player's Y-axis yaw
        Quaternion targetRotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
