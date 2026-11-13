using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public int health = 5;

    public void TakeDamage(int amount)
    {
        if (health <= 0) return;

        health -= amount;
        Debug.Log($"{name} took {amount} damage, health now {health}", this);

        if (health <= 0)
            Destroy(gameObject);

    }
}