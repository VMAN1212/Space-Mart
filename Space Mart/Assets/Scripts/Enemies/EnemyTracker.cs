using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private int sceneToLoad = 4;
    [SerializeField] private float delayBeforeLoad = 0f;

    private Queue<GameObject> enemyQueue;
    private bool loading;

    public int EnemiesRemaining => enemyQueue == null ? 0 : enemyQueue.Count;

    private void Start()
    {
        enemyQueue = new Queue<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) enemyQueue.Enqueue(enemy);
        }

    }

    private void Update()
    {
        if (loading || enemyQueue == null) return;

        int count = enemyQueue.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = enemyQueue.Dequeue();
            if (enemy != null) enemyQueue.Enqueue(enemy);
        }

        if (enemyQueue.Count == 0)
        {
            loading = true;
            Invoke(nameof(LoadNextScene), delayBeforeLoad);
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (enemy != null && !loading) enemyQueue.Enqueue(enemy);
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