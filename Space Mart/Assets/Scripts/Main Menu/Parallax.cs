using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private float parallaxAmount = 25f; // Max pixel offset
    [SerializeField] private float smoothSpeed = 8f;     // Lerp smoothing speed

    private RectTransform rectTransform;
    private Vector2 initialPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 normalizedMouse = new Vector2(
            (mousePos.x / Screen.width) - 0.5f,
            (mousePos.y / Screen.height) - 0.5f
        );

        Vector2 targetPosition = initialPosition + (normalizedMouse * parallaxAmount);

        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );
    }
}