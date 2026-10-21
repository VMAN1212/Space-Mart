using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Required for the New Input System

[RequireComponent(typeof(VideoPlayer))]
public class VideoCutsceneController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    [SerializeField]
    private string nextSceneName = "GameScene"; // Replace with your actual first gameplay scene name

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Subscribe to the event that triggers when the video finishes naturally
        videoPlayer.loopPointReached += OnVideoFinished;

        // Play the video
        videoPlayer.Play();
    }

    void Update()
    {
        // Check if the Space key was pressed this frame using the New Input System
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SkipCutscene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    public void SkipCutscene()
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        // Unsubscribe to avoid multiple triggers
        videoPlayer.loopPointReached -= OnVideoFinished;

        // Load your first gameplay scene
        SceneManager.LoadScene(nextSceneName);
    }
}