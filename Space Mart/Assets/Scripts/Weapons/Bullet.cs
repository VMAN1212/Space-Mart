using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int speed = 1;
    private Rigidbody rb;
    public EnemyLogic enemyL;
    private Coroutine deactivate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

        if (other.gameObject.tag == "Enemy")
        {
            enemyL = other.gameObject.GetComponent<EnemyLogic>();
            enemyL.health -= 1;
        }
        ReturnToPool();
    }

    private IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
