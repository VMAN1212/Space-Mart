using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShelfTracker : MonoBehaviour
{
    [SerializeField] private int sceneToLoad = 6;
    [SerializeField] private float delayBeforeLoad = 0f;

    private Queue<Shelf> shelfQueue;
    private bool loading;

    public int ShelvesRemaining => shelfQueue == null ? 0 : shelfQueue.Count;

    private void Start()
    {
        shelfQueue = new Queue<Shelf>(FindObjectsByType<Shelf>(FindObjectsSortMode.None));

        if (shelfQueue.Count == 0)
            Debug.LogWarning($"{name}: no shelves found in the scene.", this);
    }

    private void Update()
    {
        if (loading || shelfQueue == null || shelfQueue.Count == 0) return;

        int count = shelfQueue.Count;
        for (int i = 0; i < count; i++)
        {
            Shelf shelf = shelfQueue.Dequeue();
            if (shelf != null && !shelf.IsCompleted) shelfQueue.Enqueue(shelf);
        }

        if (shelfQueue.Count == 0)
        {
            loading = true;
            Invoke(nameof(LoadNextScene), delayBeforeLoad);
        }
    }

    private void LoadNextScene()
    {
        if (sceneToLoad < 0 || sceneToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"Scene index {sceneToLoad} is not in Build Settings " +
                           $"(only {SceneManager.sceneCountInBuildSettings} scenes added).", this);
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}