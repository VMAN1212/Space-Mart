using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class Boss : MonoBehaviour
{
    public int health = 5;

    public void TakeDamage(int amount)
    {
        if (health <= 0) return;

        health -= amount;
        Debug.Log($"{name} took {amount} damage, health now {health}", this);

        if (health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GoodEndingScene");
            MusicManager.Instance.PlayMusic("GoodEnd"); // Play the good ending music
        }
            
    }
}
