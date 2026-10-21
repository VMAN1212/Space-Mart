using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class BossLogic : MonoBehaviour
{
    [Header("Boss Stats")]
    public int health = 50; // Boss can have more health

    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load when the boss dies (Win/Credits scene)")]
    public string winSceneName = "WinScene";

    void Update()
    {
        if (health <= 0)
        {
            TriggerWinCondition();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Boss Health: " + health);
    }

    void TriggerWinCondition()
    {
        Debug.Log("Boss defeated! Loading win scene...");

        // Destroy the boss object so it's gone before the scene loads
        Destroy(gameObject);

        // Load your win scene (make sure it's added to File > Build Settings!)
        SceneManager.LoadScene("Supermarket");
    }
}