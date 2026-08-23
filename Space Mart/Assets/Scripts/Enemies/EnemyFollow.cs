using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform Player;
    public float range = 5f;


    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance < range)
        {
            enemy.SetDestination(Player.position);
        }
    }
}
