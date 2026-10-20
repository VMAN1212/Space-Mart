using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    public Camera mainCam;
    public float range = 5f;
    public GameObject actionUI;
    public TextMeshProUGUI actionText;
    public PlayerInput playerInput;
    public InputActionReference action;

    [Header("Reload Prompt Integration")]
    public Ammo playerAmmo;
    public InputActionReference reloadAction;

    private IInteractable interactable;
    private EquipScript equipped;

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.onControlsChanged += HandleControlsChanged;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.onControlsChanged -= HandleControlsChanged;
        }
    }

    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueOpen())
        {
            if (actionUI != null) actionUI.SetActive(false);
            interactable = null;
            return;
        }

        RaycastHit hit;
        bool didHit = Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range);

        if (didHit)
        {
            interactable = hit.transform.GetComponent<IInteractable>();
        }
        else
        {
            interactable = null;
        }

        if (interactable != null)
        {
            actionUI.SetActive(true);
            actionText.text = "Press " + GetCurrentBinding(action) + " to " + interactable.getPrompt();
        }
        else if (playerAmmo != null && playerAmmo.currentAmmo == 0 && playerAmmo.reserveAmmo > 0)
        {
            actionUI.SetActive(true);
            actionText.text = "Press " + GetCurrentBinding(reloadAction) + " to reload.";
        }
        else
        {
            actionUI.SetActive(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (interactable != null)
        {
            if (interactable is EquipScript && equipped != null)
            {
                equipped.Drop();
                equipped = null;
            }

            interactable.Interact();

            if (interactable is EquipScript equipable)
            {
                equipped = equipable;
            }

            interactable = null;
            return;
        }

        if (equipped != null)
        {
            equipped.Drop();
            equipped = null;
        }
    }

    private string GetCurrentBinding(InputActionReference actionRef)
    {
        if (actionRef == null) return "R";

        if (playerInput != null)
        {
            string scheme = playerInput.currentControlScheme;
            return actionRef.action.GetBindingDisplayString(InputBinding.MaskByGroup(scheme));
        }

        return actionRef.action.GetBindingDisplayString();
    }

    private void HandleControlsChanged(PlayerInput input)
    {
        if (interactable != null)
        {
            actionText.text = "Press " + GetCurrentBinding(action) + " to " + interactable.getPrompt();
        }
    }
}