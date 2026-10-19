using UnityEngine;

public class FirstPersonShockwave : MonoBehaviour
{
    [Header("Scaling Settings")]
    public float maxScale = 60f;
    public float expandSpeed = 20f;

    private float currentScale;

    void Start()
    {
        // Grab whatever scale the prefab currently has in the editor to start fresh
        currentScale = transform.localScale.x;
    }

    void Update()
    {
        // Increase scale over time
        currentScale += expandSpeed * Time.deltaTime;

        // Apply scale (keeping Y thin so it's a flat ground wave)
        transform.localScale = new Vector3(currentScale, 0.1f, currentScale);

        // Debug check (remove later if your console gets cluttered)
        // Debug.Log("Shockwave Scale: " + currentScale);

        // Destroy when it hits max size
        if (currentScale >= maxScale)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by shockwave!");
            // other.GetComponent<PlayerHealth>()?.TakeDamage(1);
        }
    }
}