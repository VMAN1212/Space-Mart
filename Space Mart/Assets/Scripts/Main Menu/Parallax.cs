using UnityEngine;
using UnityEngine.InputSystem;

public class Parallax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private float parallaxAmount = 25f; 
    [SerializeField] private float smoothSpeed = 8f;     
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        Vector2 inputVector = Vector2.zero;

       
        if (Gamepad.current != null)
        {
            inputVector = Gamepad.current.rightStick.ReadValue();
        }

      
        if (inputVector == Vector2.zero && Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

      
            inputVector = new Vector2(
                ((mousePos.x / Screen.width) - 0.5f) * 2f,
                ((mousePos.y / Screen.height) - 0.5f) * 2f
            );
        }

       
        Vector2 targetPosition = initialPosition + (inputVector * parallaxAmount);

        
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );
    }
}