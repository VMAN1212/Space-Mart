using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour, IInteractable
{
    public float drop = 2f;
    public Transform playerTransform;
    public GameObject item;
    public bool isEquipped = false;

    public string getPrompt()
    {
        return "pick up";
    }

    public void Interact()
    {
        Debug.Log("Interact called on Target, isEquipped=" + isEquipped);
        if (isEquipped) return;
        EquipObject();
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!isEquipped) return;
        UnequipObject();
    }

    private void EquipObject()
    {
        isEquipped = true;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        item.transform.position = playerTransform.position;
        item.transform.rotation = playerTransform.rotation;
        item.transform.SetParent(playerTransform);
    }

    private void UnequipObject()
    {
        isEquipped = false;
        playerTransform.DetachChildren();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(playerTransform.transform.forward * drop, ForceMode.Impulse);
        }
    }
}