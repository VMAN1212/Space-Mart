using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI & Visuals")]
    public Slider healthSlider;           // Drag your HealthSlider here in the Inspector
    public Image damageFlashPanel;        // Drag your DamageFlashPanel here in the Inspector
    public float flashDuration = 0.2f;
    public float maxAlpha = 0.6f;

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize the slider if assigned
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // Auto-find damage flash panel if missing
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Keep health between 0 and max

        Debug.Log("Player took damage! Current Health: " + currentHealth);

        // Update the health slider UI
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Trigger the visual red flash
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
        // Add your game over / reload scene logic here
        SceneManager.LoadScene("Supermarket");
    }
}