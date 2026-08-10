using UnityEngine;

public class Target : MonoBehaviour
{
    public float drop = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Commence(float amount)
    {
        drop -= amount;
        if (drop < 0)
        {
            Go();
        }

    }

    void Go()
    {
        Destroy(gameObject);
    }
}
