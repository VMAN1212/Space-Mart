using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;
    private float originalMoveSpeed;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        HandleMovement();
        HandleLook();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {

        lookInput = context.ReadValue<Vector2>();
    }
    // Handles the player's movement and gravity.
    public void HandleMovement()
    {
        // Creates the horizontal movement direction.
        Vector3 move =
        transform.right * moveInput.x +
        transform.forward * moveInput.y;
        // Moves the player horizontally.
        controller.Move(move * moveSpeed * Time.deltaTime);
        // Keeps the player connected to the ground.
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // Gravity must run every frame, not only while grounded.
        velocity.y += gravity * Time.deltaTime;
        // Applies the vertical movement for jumping and falling.
        controller.Move(velocity * Time.deltaTime);
    }
    // Handles the player's camera and body rotation.
    public void HandleLook()
    {
        // Calculates horizontal camera movement using the look input
        // and the selected sensitivity.
        float mouseX = lookInput.x * lookSensitivity;
        // Calculates vertical camera movement using the look input
        // and the selected sensitivity.
        float mouseY = lookInput.y * lookSensitivity;
        // Subtracts the vertical mouse movement from the camera rotation.
        // Subtraction makes moving the mouse upwards look upwards.
        verticalRotation -= mouseY;
        // Limits the vertical camera rotation so that the player
        // cannot rotate the camera completely over their head.
        verticalRotation = Mathf.Clamp(
        verticalRotation,
        -verticalLookLimit,
        verticalLookLimit
        );
        // Rotates only the camera up and down.
        cameraTransform.localRotation =
        Quaternion.Euler(verticalRotation, 0f, 0f);
        // Rotates the entire player GameObject left and right.
        transform.Rotate(Vector3.up * mouseX);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded) // Check that the Jump action was successfully performed and that the player is currently standing on the ground.
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //
                                                                 //Calculates the upward speed needed for the player to reach the chosen jump
                                                                 //height while accounting for gravity.
        }
    }
}
    