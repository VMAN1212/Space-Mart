using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed => sprintIntput ? sprintSpeed : walkSpeed;
    [SerializeField] float acceleration = 10f;

    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;

    [Space(15)]
    [SerializeField] float jumpHeight = 1.5f;

    public bool Sprinting
    {
        get
        {
            return sprintIntput && currentSpeed > 0.1f;
        }
    }

    [Header("Look Settings")]
    public Vector2 lookSensitivity = new Vector2(1.5f, 1.5f);

    public float PitchLimit = 85f;

    [SerializeField] float currentPitch = 0f;

    public float CurrentPitch
    {
        get => currentPitch;

        set
        {
            currentPitch = Mathf.Clamp(value, -PitchLimit, PitchLimit);
        }
    }

    [Header("Camera Settings")]
    [SerializeField] float cameraNormalFOV = 60f;
    [SerializeField] float cameraSprintFOV = 80f;
    [SerializeField] float cameraFOVSmoothing = 1f;

    public float targetCameraFOV
    {
        get
        {
            return Sprinting ? cameraSprintFOV : cameraNormalFOV;
        }
    }

    [Header("Physics Parameters")]
    [SerializeField] float gravityScale = 3f;

    public float verticalVelocity = 0f; 

    public Vector3 currentVelocity { get; private set; }
    public float currentSpeed { get; private set; }

    public bool isGrounded => controller.isGrounded;

    [Header("Inputs")]
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool sprintIntput;

    [Header("Components")]

    public Camera fpCamera;
    [SerializeField] CharacterController controller;

    void OnValidate()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveUpdate();
        LookUpdate();
        CameraUpdate();
    }

    public void TryJump()
    {
        if(isGrounded == false)
        {
            return;
        }

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravityScale * Physics.gravity.y);
    }

    void MoveUpdate()
    {
        Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
        motion.y = 0f;
        motion.Normalize();

        if (motion.sqrMagnitude >= 0.01f)
        {
            currentVelocity = motion * moveSpeed;
        }
        else
        {
            currentVelocity = Vector3.zero;
        }

        if (isGrounded && verticalVelocity <= 0.01f)
        {
            verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += gravityScale * Physics.gravity.y * Time.deltaTime;
        }

        Vector3 fullVelocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);

        controller.Move(fullVelocity * Time.deltaTime);
    }

    void LookUpdate()
    {
        Vector2 input  = new Vector2(lookInput.x * lookSensitivity.x, lookInput.y * lookSensitivity.y);

        //looking up and down
        CurrentPitch -= input.y;

        fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

        //looking left and right
        transform.Rotate(Vector3.up * input.x);
    }

    void CameraUpdate()
    {
        float targetFOV = cameraNormalFOV;

        if(Sprinting)
        {
            float speedRatio = currentSpeed / sprintSpeed;

            targetFOV = Mathf.Lerp(cameraNormalFOV, cameraSprintFOV, speedRatio);
        }

        fpCamera.fieldOfView = Mathf.Lerp(fpCamera.fieldOfView, targetFOV, cameraFOVSmoothing * Time.deltaTime);
    }

}
