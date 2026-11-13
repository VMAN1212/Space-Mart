using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public int damage = 1;

    private Rigidbody rb;
    private Coroutine deactivate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // stops fast bullets passing through
    }

    private void OnEnable()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearVelocity = transform.forward * speed;

        if (deactivate != null) StopCoroutine(deactivate);
        deactivate = StartCoroutine(DeactivateAfterTime(4f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("Player")) return;

        Debug.Log($"Bullet hit: {other.name}", other); // click the log to highlight what was hit

        // Works even when the collider is on a child of the enemy
        EnemyLogic enemy = other.GetComponentInParent<EnemyLogic>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        Boss enemyBoss = other.GetComponentInParent<Boss>();
        if (enemyBoss != null)
            enemyBoss.TakeDamage(damage);

        ReturnToPool();
    }

    private IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (deactivate != null) { StopCoroutine(deactivate); deactivate = null; }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}