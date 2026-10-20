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

    [Tooltip("Multiplier applied specifically to controllers to make joysticks feel snappy.")]
    public float gamepadSensMultiplier = 50f;

    private float xRotation;
    private float yRotation;

    [Header("Wall Run Tilt")]
    public float wallTilt = 10f;
    private float currentTilt;
    private float targetTilt;
    public float tiltTransitionSpeed = 10f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable() => lookInput.action.Enable();
    private void OnDisable() => lookInput.action.Disable();

    private void Update()
    {
        Vector2 lookValue = lookInput.action.ReadValue<Vector2>();

        // Check if the active input device is a Gamepad and scale the input accordingly
        float currentMultiplier = 1f;
        if (lookInput.action.activeControl?.device is Gamepad)
        {
            currentMultiplier = gamepadSensMultiplier;
        }

        // Apply sensitivity and scaling
        float inputX = lookValue.x * Time.deltaTime * sensX * currentMultiplier;
        float inputY = lookValue.y * Time.deltaTime * sensY * currentMultiplier;

        yRotation += inputX;
        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltTransitionSpeed * Time.deltaTime);

        // Apply rotation with Z-axis tilt
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, currentTilt);
    }

    public void SetWallTilt(float tilt)
    {
        targetTilt = tilt;
    }
}