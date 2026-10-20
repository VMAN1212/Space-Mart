using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Panels")]
    public GameObject dialogueBoxUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI continuePromptText;

    [Header("UI To Disable During Dialogue")]
    public GameObject[] uiToHide;

    [Header("Animation")]
    public Animator animator;

    [Header("New Input System")]
    public PlayerInput playerInput;
    public InputActionReference continueAction;

    [Header("Input Control")]
    public PlayerInput jumpInput;

    private Queue<DialogueLine> dialogueLinesQueue;
    private bool isDialogueOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogueLinesQueue = new Queue<DialogueLine>();
    }

    private void OnEnable()
    {
        if (continueAction != null)
        {
            continueAction.action.Enable();
            continueAction.action.performed += OnContinuePerformed;
        }

        if (playerInput != null)
        {
            playerInput.onControlsChanged += HandleControlsChanged;
        }
    }

    private void OnDisable()
    {
        if (continueAction != null)
        {
            continueAction.action.performed -= OnContinuePerformed;
            continueAction.action.Disable();
        }

        if (playerInput != null)
        {
            playerInput.onControlsChanged -= HandleControlsChanged;
        }
    }

    private void OnContinuePerformed(InputAction.CallbackContext context)
    {
        if (!isDialogueOpen) return;
        DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueOpen = true;

        SetOtherUIActive(false);

        dialogueBoxUI.SetActive(true);

        if (animator != null)
        {
            animator.SetBool("isOpen", true);
        }

        UpdateContinuePrompt();

        dialogueLinesQueue.Clear();

        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            dialogueLinesQueue.Enqueue(line);
        }

        DisplayNextSentence();

        if (jumpInput != null)
        {
            jumpInput.actions["Jump"].Disable();
        }
    }

    public void DisplayNextSentence()
    {
        if (dialogueLinesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = dialogueLinesQueue.Dequeue();

        nameText.text = currentLine.speakerName;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        isDialogueOpen = false;

        if (animator != null)
        {
            animator.SetBool("isOpen", false);
        }
        else
        {
            dialogueBoxUI.SetActive(false);
        }

        SetOtherUIActive(true);

        if (jumpInput != null)
        {
            jumpInput.actions["Jump"].Disable();
        }
    }

    private void SetOtherUIActive(bool isActive)
    {
        if (uiToHide == null) return;

        foreach (GameObject uiElement in uiToHide)
        {
            if (uiElement != null)
            {
                uiElement.SetActive(isActive);
            }
        }
    }

    private void UpdateContinuePrompt()
    {
        if (continuePromptText != null && continueAction != null)
        {
            string bindingName = GetCurrentBinding();
            continuePromptText.text = "Press [" + bindingName + "] to continue";
        }
    }

    private string GetCurrentBinding()
    {
        if (continueAction == null) return "Space";

        if (playerInput != null)
        {
            string scheme = playerInput.currentControlScheme;
            return continueAction.action.GetBindingDisplayString(InputBinding.MaskByGroup(scheme));
        }

        return continueAction.action.GetBindingDisplayString();
    }

    private void HandleControlsChanged(PlayerInput input)
    {
        UpdateContinuePrompt();
    }

    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}