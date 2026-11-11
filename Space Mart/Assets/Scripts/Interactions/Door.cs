using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    [SerializeField] private string promptMessage = "enter Door";

    public string getPrompt()
    {
        return promptMessage;
    }

    public void Interact()
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            MusicManager.Instance.PlayMusic("Supermarket"); // Play the intro music when transitioning to the next scene
        }

    }
}