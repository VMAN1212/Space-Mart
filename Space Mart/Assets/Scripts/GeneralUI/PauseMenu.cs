using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; 

public class PauseMenuManager : MonoBehaviour
{
    [Header("Separate Pause Canvas")]
    public GameObject pauseCanvas;

    [Header("Gamepad UI Navigation")]
    public GameObject firstSelectedButton; 

    private bool isPaused = false;

    private void Awake()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(true);
        }

        if (EventSystem.current != null && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}