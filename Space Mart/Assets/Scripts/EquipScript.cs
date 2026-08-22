using UnityEngine;
using UnityEngine.InputSystem;

public class EquipScript : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject item;
    public Camera mainCamera;
    public float range = 3f;
    public float force = 3f;

    private bool isEquipped = false;

    void Start()
    {
        item = null;
        isEquipped = false;
        
    }

    // Called automatically by the PlayerInput component when "Interact" is pressed
    // Change InputValue to InputAction.CallbackContext
    public void OnInteract(InputAction.CallbackContext context)
    {
        // context.performed means the button was fully pressed down this frame
        if (context.performed)
        {
            if (isEquipped)
            {
                UnequipObject();
            }
            else
            {
                TryPickUp();
            }
        }
    }

    private void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                item = hit.transform.gameObject;
                EquipObject();
            }
        }
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

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(mainCamera.transform.forward * force, ForceMode.Impulse);
        }
        item = null;
    }
}