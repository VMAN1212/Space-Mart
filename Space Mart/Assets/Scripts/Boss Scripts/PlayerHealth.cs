using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // Required for scene management

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Damage Flash Settings")]
    public Image damageFlashPanel; // Drag your DamageFlashPanel here in the Inspector
    public float flashDuration = 0.2f;
    public float maxAlpha = 0.6f;   // How bright the red flash gets (0 to 1)

    void Start()
    {
        currentHealth = maxHealth;

        // If you forgot to drag it in, try to find it automatically by name
        if (damageFlashPanel == null)
        {
            GameObject panelObj = GameObject.Find("DamageFlashPanel");
            if (panelObj != null)
                damageFlashPanel = panelObj.GetComponent<Image>();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player took damage! Current Health: " + currentHealth);

        // Trigger the visual flash
        if (damageFlashPanel != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRoutine()
    {
        // Flash to max alpha instantly
        SetPanelAlpha(maxAlpha);

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(maxAlpha, 0f, elapsed / flashDuration);
            SetPanelAlpha(currentAlpha);
            yield return null;
        }

        SetPanelAlpha(0f);
    }

    void SetPanelAlpha(float alpha)
    {
        Color c = damageFlashPanel.color;
        c.a = alpha;
        damageFlashPanel.color = c;
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // Add your respawn or game over logic here
        SceneManager.LoadScene("Supermarket");
    }
}