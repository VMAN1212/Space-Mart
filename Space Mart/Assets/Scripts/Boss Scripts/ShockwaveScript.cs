using UnityEngine;

public class FirstPersonShockwave : MonoBehaviour
{
    [Header("Scaling Settings")]
    public float maxScale = 60f;
    public float expandSpeed = 20f;

    [Header("Audio")]
    public string boomSoundID = "Boom"; // Must match the string ID in your SoundLibrary

    private float currentScale;

    void Start()
    {
        // Grab whatever scale the prefab currently has in the editor to start fresh
        currentScale = transform.localScale.x;

        // Play the boom sound effect instantly when the shockwave spawns
        PlayBoomSound();
    }

    void Update()
    {
        // Increase scale over time
        currentScale += expandSpeed * Time.deltaTime;

        // Apply scale (keeping Y thin so it's a flat ground wave)
        transform.localScale = new Vector3(currentScale, 0.01f, currentScale);

        // Destroy when it hits max size
        if (currentScale >= maxScale)
        {
            Destroy(gameObject);
        }
    }

    private void PlayBoomSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound2D("Attack");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by shockwave!");
            other.GetComponent<PlayerHP>()?.TakeDamage(10);
        }
    }
}