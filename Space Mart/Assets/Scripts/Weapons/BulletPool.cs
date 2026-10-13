using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    [SerializeField] GameObject bullet;
    [SerializeField] private int poolsize = 12;
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitialisePool();
    }

    private void InitialisePool()
    {
        for (int i = 0; i < poolsize; i++)
        {
            GameObject obj = Instantiate(bullet);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        GameObject egg;

        if (pool.Count > 0)
        {
            egg = pool.Dequeue();
        }
        else
        {
            egg = Instantiate(bullet);
        }

        egg.transform.position = position;
        egg.transform.rotation = rotation;
        egg.SetActive(true);

        return egg;
    }

    public void ReturnBullet(GameObject egg)
    {
        egg.SetActive(false);
        pool.Enqueue(egg);
    }

}
