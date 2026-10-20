using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("Dialogue Sequence")]
    public Dialogue[] dialogueSteps;

    [Header("Prompt Settings")]
    [SerializeField] private string interactionPrompt = "talk.";

    private int currentDialogueIndex = 0;

    public string getPrompt()
    {
        return interactionPrompt;
    }

    public void Interact()
    {
        if (dialogueSteps == null || dialogueSteps.Length == 0) return;

        int safeIndex = Mathf.Clamp(currentDialogueIndex, 0, dialogueSteps.Length - 1);

        Dialogue activeDialogue = dialogueSteps[safeIndex];

        if (activeDialogue != null && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(activeDialogue);
        }
    }

    
    public void AdvanceDialogueStep()
    {
        if (currentDialogueIndex < dialogueSteps.Length - 1)
        {
            currentDialogueIndex++;
            Debug.Log("NPC advanced to dialogue step index: " + currentDialogueIndex);
        }
    }

    public void SetDialogueStep(int stepIndex)
    {
        if (stepIndex >= 0 && stepIndex < dialogueSteps.Length)
        {
            currentDialogueIndex = stepIndex;
        }
    }
}