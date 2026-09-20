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
            actionText.text = "Press " + GetCurrentBinding() + " to " + interactable.getPrompt();
        }
        else
        {
            actionUI.SetActive(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (equipped != null)
        {
            equipped.Drop();
            equipped = null;
            return;
        }

        if (interactable != null)
        {
            interactable.Interact();

            if (interactable is EquipScript equipable)
            {
                equipped = equipable;
            }

            interactable = null;
        }
    }

    private string GetCurrentBinding()
    {
        string scheme = playerInput.currentControlScheme;
        return action.action.GetBindingDisplayString(InputBinding.MaskByGroup(scheme));
    }

    private void HandleControlsChanged(PlayerInput input)
    {
        if (interactable != null)
        {
            actionText.text = "Press " + GetCurrentBinding() + " to " + interactable.getPrompt();
        }
    }

}