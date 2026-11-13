using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShelfTracker : MonoBehaviour
{
    [SerializeField] private int sceneToLoad = 6;
    [SerializeField] private float delayBeforeLoad = 0f;

    [SerializeField] private List<GameObject> Shelves = new List<GameObject>();

    private readonly List<Shelf> remaining = new List<Shelf>();
    private bool loading;

    public int ShelvesRemaining => remaining.Count;

    private void Start()
    {
        foreach (GameObject go in Shelves)
        {
            if (go == null) continue;

            Shelf shelf = go.GetComponent<Shelf>();
            if (shelf == null)
            {
                Debug.LogWarning($"{go.name} is in the Shelves list but has no Shelf component.", go);
                continue;
            }

            if (!remaining.Contains(shelf))
                remaining.Add(shelf);
        }

        if (remaining.Count == 0)
        {
            Debug.LogWarning($"{name}: no valid shelves assigned in the Shelves list.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (loading) return;

        remaining.RemoveAll(shelf => shelf == null || shelf.IsCompleted);

        if (remaining.Count == 0)
        {
            loading = true;
            Invoke(nameof(LoadNextScene), delayBeforeLoad);
            MusicManager.Instance.PlayMusic("GoodEnd");
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