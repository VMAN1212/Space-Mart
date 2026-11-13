using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerCam : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference lookInput;

    [Header("Sensitivity")]
    public float sensX = 400f;
    public float sensY = 400f;
    public float gamepadSensMultiplier = 50f;

    private float xRotation;
    private float yRotation;

    [Header("Wall Run Tilt")]
    public float wallTilt = 10f;
    private float currentTilt;
    private float targetTilt;
    public float tiltTransitionSpeed = 10f;

    [Header("FOV Settings")]
    public Camera playerCam;
    public float normalFOV = 60f;
    public float sprintFOV = 75f;
    public float fovTransitionSpeed = 10f;
    private float targetFOV;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCam == null) playerCam = GetComponent<Camera>();
        if (playerCam != null) normalFOV = playerCam.fieldOfView;
        targetFOV = normalFOV;
    }

    private void OnEnable() => lookInput.action.Enable();
    private void OnDisable() => lookInput.action.Disable();

    private void Update()
    {
        Vector2 lookValue = lookInput.action.ReadValue<Vector2>();

        float currentMultiplier = 1f;
        if (lookInput.action.activeControl?.device is Gamepad)
        {
            currentMultiplier = gamepadSensMultiplier;
        }

        float inputX = lookValue.x * Time.deltaTime * sensX * currentMultiplier;
        float inputY = lookValue.y * Time.deltaTime * sensY * currentMultiplier;

        yRotation += inputX;
        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Smoothly interpolate FOV
        if (playerCam != null)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);
        }

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltTransitionSpeed * Time.deltaTime);

        // Apply rotation with Z-axis tilt
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, currentTilt);
    }

    public void SetWallTilt(float tilt)
    {
        targetTilt = tilt;
    }

    public void SetFOV(float newFOV)
    {
        targetFOV = newFOV;
    }
}