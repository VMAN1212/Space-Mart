using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private int gameSceneIndex = 1;

    [SerializeField] private GameObject firstSelectedButton;

    private void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (EventSystem.current != null && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void ExitGame()
    {
    
        Application.Quit();

    }
}
