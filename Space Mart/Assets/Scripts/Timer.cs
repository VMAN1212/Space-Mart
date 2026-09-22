using UnityEngine;
using TMPro; // Required for TextMeshPro UI elements
using UnityEngine.SceneManagement; // Required for scene management

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f; // Total time in seconds
    public bool isTimerRunning = false;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText; // Reference to your TextMeshPro component

    private void Start()
    {
        // Starts the timer automatically when the scene begins
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                // Subtract the time passed since the last frame
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                isTimerRunning = false;
                OnTimerEnd();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Calculate minutes and seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Format string as "00:00"
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTimerEnd()
    {
        Debug.Log("Timer has ended. Implement your end-of-timer logic here.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
