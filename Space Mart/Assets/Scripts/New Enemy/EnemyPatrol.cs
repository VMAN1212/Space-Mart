using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent agent;
    public Transform player;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int currentWaypointIndex = 0;
    public float patrolSpeed = 3.5f;

    [Header("Chase Settings")]
    public float chaseRange = 8f;
    public float chaseSpeed = 5.5f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Auto-find player if not assigned
        if (player == null && GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        // Move to the first waypoint
        GotoNextWaypoint();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            // === CHASE MODE ===
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            // === PATROL MODE ===
            agent.speed = patrolSpeed;

            // Check if enemy has reached the current waypoint
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextWaypoint();
            }
        }
    }

    private void GotoNextWaypoint()
    {
        if (patrolPoints.Length == 0) return;

        // Set destination to current waypoint
        agent.SetDestination(patrolPoints[currentWaypointIndex].position);

        // Advance to the next index, looping back to 0 if at the end
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Length;
    }

    // Visualize chase range in the Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}