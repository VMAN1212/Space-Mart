using System.Collections;
using UnityEngine;

public class BossSlamAttackBothFists : MonoBehaviour
{
    [Header("Timing")]
    public float attackInterval = 4f;
    public float telegraphTime = 1f;
    public float slamSpeed = 15f;

    [Header("Shockwave")]
    public GameObject shockwavePrefab;

    [Header("Fist 1 (Left)")]
    public Transform fist1Transform;
    public Transform fist1RestPos;
    public Transform fist1SlamPos;

    [Header("Fist 2 (Right)")]
    public Transform fist2Transform;
    public Transform fist2RestPos;
    public Transform fist2SlamPos;

    private bool useFirstFist = true; // Tracks which fist attacks next

    void Start()
    {
        StartCoroutine(BossRoutine());
    }

    IEnumerator BossRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            // Choose which fist to use this turn
            Transform activeFist = useFirstFist ? fist1Transform : fist2Transform;
            Transform activeRest = useFirstFist ? fist1RestPos : fist1RestPos; // Wait, let's map correctly below!
            Transform activeSlam = useFirstFist ? fist1SlamPos : fist2SlamPos;
            Transform currentRest = useFirstFist ? fist1RestPos : fist2RestPos;

            // 1. Telegraph Phase (Warning shake on the active fist)
            yield return StartCoroutine(TelegraphPhase(activeFist));

            // 2. Slam Down
            yield return StartCoroutine(MoveFistTo(activeFist, activeSlam.position));

            // 3. Impact & Spawn Shockwave
            PerformSlam(activeSlam.position);

            // 4. Return Fist to Rest Position
            yield return StartCoroutine(MoveFistTo(activeFist, currentRest.position));

            // Flip the switch for the next attack
            useFirstFist = !useFirstFist;
        }
    }

    IEnumerator TelegraphPhase(Transform fist)
    {
        float timer = 0f;
        Vector3 originalPos = fist.position;

        while (timer < telegraphTime)
        {
            fist.position = originalPos + (Random.insideUnitSphere * 0.2f);
            timer += Time.deltaTime;
            yield return null;
        }
        fist.position = originalPos;
    }

    IEnumerator MoveFistTo(Transform fist, Vector3 targetPos)
    {
        while (Vector3.Distance(fist.position, targetPos) > 0.05f)
        {
            fist.position = Vector3.MoveTowards(fist.position, targetPos, slamSpeed * Time.deltaTime);
            yield return null;
        }
        fist.position = targetPos;
    }

    void PerformSlam(Vector3 spawnPos)
    {
        if (shockwavePrefab != null)
        {
            Instantiate(shockwavePrefab, spawnPos, Quaternion.identity);
        }
        Debug.Log("SLAM! Shockwave spawned from fist.");
    }
}