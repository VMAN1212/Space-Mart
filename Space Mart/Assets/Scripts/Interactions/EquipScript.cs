using UnityEngine;
using UnityEngine.InputSystem;

public class EquipScript : MonoBehaviour, IInteractable 
{
    public Transform playerTransform;
    public float force = 3f;
    private bool isEquipped = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public string getPrompt()
    {
        return "pick up.";
    }

    public void Interact()
    {
        if (isEquipped) return;
        EquipObject();
    }

    public void Drop()
    {
        if (!isEquipped) return;
        UnequipObject();
    }

    private void EquipObject()
    {
        isEquipped = true;
        if (rb != null) rb.isKinematic = true;

        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
            transform.rotation = playerTransform.rotation;
            transform.SetParent(playerTransform);
        }
    }

    private void UnequipObject()
    {
        isEquipped = false;
        if (playerTransform != null)
        {
            playerTransform.DetachChildren();
        }
        else
        {
            transform.SetParent(null);
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            if (playerTransform != null)
            {
                rb.AddForce(playerTransform.forward * force, ForceMode.Impulse);
            }
        }
    }
}
