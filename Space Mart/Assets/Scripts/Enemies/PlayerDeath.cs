using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Header("Death / Stun Settings")]
    [SerializeField] private string enemyName = "Enemy";
    [TextArea(2, 5)]
    [SerializeField] private string deathMessage = "You've been caught! Stunned for 5 seconds...";
    [SerializeField] private float stunDuration = 5f;
    [SerializeField] private float stunCooldown = 2f;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody playerRb;

    private bool isStunned = false;
    private bool canBeStunned = true;
    private RigidbodyConstraints originalConstraints;

    private void OnCollisionEnter(Collision other)
    {
        if (isStunned || !canBeStunned) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(UnskippableStunRoutine());
        }
    }

    private IEnumerator UnskippableStunRoutine()
    {
        isStunned = true;
        canBeStunned = false;

        if (playerRb != null)
        {
            originalConstraints = playerRb.constraints;
            playerRb.constraints = RigidbodyConstraints.FreezePosition | originalConstraints;
            playerRb.linearVelocity = Vector3.zero;
        }

        if (playerMovement != null)
        {
            playerMovement.canMove = false;
            playerMovement.canJump = false;
        }

        if (DialogueManager.Instance != null)
        {
            DialogueLine deathLine = new DialogueLine
            {
                speakerName = enemyName,
                sentence = deathMessage
            };

            Dialogue stunDialogue = new Dialogue
            {
                dialogueLines = new DialogueLine[] { deathLine }
            };

            DialogueManager.Instance.StartDialogue(stunDialogue);

            if (DialogueManager.Instance.continuePromptText != null)
            {
                DialogueManager.Instance.continuePromptText.gameObject.SetActive(false);
            }
        }

        yield return new WaitForSeconds(stunDuration);

        if (DialogueManager.Instance != null)
        {
            if (DialogueManager.Instance.continuePromptText != null)
            {
                DialogueManager.Instance.continuePromptText.gameObject.SetActive(true);
            }

            if (DialogueManager.Instance.dialogueBoxUI != null)
            {
                DialogueManager.Instance.dialogueBoxUI.SetActive(false);
            }

            if (DialogueManager.Instance.animator != null)
            {
                DialogueManager.Instance.animator.SetBool("isOpen", false);
            }
        }

        if (playerRb != null)
        {
            playerRb.constraints = originalConstraints;
        }

        if (playerMovement != null)
        {
            playerMovement.canMove = true;
            playerMovement.canJump = true;
        }

        isStunned = false;

        yield return new WaitForSeconds(stunCooldown);
        canBeStunned = true;
    }
}