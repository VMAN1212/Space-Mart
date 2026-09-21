using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Controller))]
public class FPPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Controller controller;

    public void OnMove(InputAction.CallbackContext context)
    {
        controller.moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {

       controller.lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        controller.sprintIntput = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded) 
        {
            controller.TryJump();           
        }
    }

    void OnValidate()
    {
        if (controller == null) controller = GetComponent<Controller>();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
