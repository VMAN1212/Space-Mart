using System.Collections;
using UnityEngine;

public class BossSlamAttack : MonoBehaviour
{
    [Header("Timing")]
    public float attackInterval = 4f; // Time between attacks
    public float telegraphTime = 1f;  // Warning time before the slam
    public float slamSpeed = 15f;     // How fast the fist moves

    [Header("References")]
    public GameObject shockwavePrefab;
    public Transform fistTransform;   // Your boss's fist GameObject
    public Transform restPosition;    // Where the fist normally sits
    public Transform slamPosition;    // Where the fist hits the ground

    void Start()
    {
        StartCoroutine(BossRoutine());
    }

    IEnumerator BossRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            // 1. Telegraph Phase (Warning shake)
            yield return StartCoroutine(TelegraphPhase());

            // 2. Slam Down
            yield return StartCoroutine(MoveFistTo(slamPosition.position));

            // 3. Impact & Spawn Shockwave
            PerformSlam();

            // 4. Return Fist to Rest Position
            yield return StartCoroutine(MoveFistTo(restPosition.position));
        }
    }

    IEnumerator TelegraphPhase()
    {
        float timer = 0f;
        Vector3 originalPos = fistTransform.position;

        while (timer < telegraphTime)
        {
            // Tiny rattle effect for tension
            fistTransform.position = originalPos + (Random.insideUnitSphere * 0.2f);
            timer += Time.deltaTime;
            yield return null;
        }
        fistTransform.position = originalPos;
    }

    IEnumerator MoveFistTo(Vector3 targetPos)
    {
        while (Vector3.Distance(fistTransform.position, targetPos) > 0.05f)
        {
            fistTransform.position = Vector3.MoveTowards(fistTransform.position, targetPos, slamSpeed * Time.deltaTime);
            yield return null;
        }
        fistTransform.position = targetPos;
    }

    void PerformSlam()
    {
        if (shockwavePrefab != null && slamPosition != null)
        {
            Instantiate(shockwavePrefab, slamPosition.position, Quaternion.identity);
        }
        Debug.Log("SLAM! Shockwave spawned.");
    }
}
