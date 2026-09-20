using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 1;
    private Rigidbody rb;
    public EnemyLogic enemyL; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyL = other.gameObject.GetComponent<EnemyLogic>();
            enemyL.health -= 1;
            Destroy(gameObject);
        }
    }
}
