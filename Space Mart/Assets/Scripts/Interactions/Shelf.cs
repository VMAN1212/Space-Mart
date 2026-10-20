using UnityEngine;
using TMPro;

public class Shelf : MonoBehaviour
{
    [Header("Shelf Configuration")]
    [SerializeField] private string acceptedItemType = "Apple"; 
    [SerializeField] private Transform[] placePoint;
    [SerializeField] private TextMeshProUGUI counterText;

    [SerializeField] private NPCInteraction npcInteraction;

    private bool[] isFull;
    private int nextSlot = 0;
    private int placedItems = 0;
    private int requiredItems;

    private void Awake()
    {
        isFull = new bool[placePoint.Length];
        requiredItems = placePoint.Length;
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Occupied()) return;

        EquipScript equipped = other.GetComponent<EquipScript>();
        if (equipped == null) return;

        if (equipped.itemType != acceptedItemType) return;

        Snap(other.gameObject);
    }

    private void Snap(GameObject item)
    {
        Transform slot = placePoint[nextSlot];
        isFull[nextSlot] = true;
        nextSlot++;
        placedItems++;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        item.transform.SetParent(null);
        item.transform.position = slot.position;
        item.transform.rotation = slot.rotation;

        UpdateUI();

        if (placedItems >= requiredItems)
        {
            TaskCompleted();
        }
    }

    private bool Occupied() => nextSlot >= placePoint.Length;

    private void UpdateUI()
    {
        if (counterText != null)
        {
            counterText.text = placedItems + "/" + requiredItems + " " + acceptedItemType + "s placed.";
        }
    }

    private void TaskCompleted()
    {
        if (counterText != null)
        {
            counterText.text = acceptedItemType + " Shelf Completed!";
        }

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.ProgressTutorial();
        }
    }
}