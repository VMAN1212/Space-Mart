using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField] private Transform[] placePoint;
    private bool[] isFull;
    private int nextSlot = 0;

    private void Awake()
    {
        isFull = new bool[placePoint.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Occupied()) return;

        EquipScript equipped = other.GetComponent<EquipScript>();
        if (equipped == null) return;

        Snap(other.gameObject);
    }

    private void Snap(GameObject item)
    {
        Transform slot = placePoint[nextSlot];
        isFull[nextSlot] = true;
        nextSlot++;
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

        ShelfUI.Instance.Placed();
    }

    private bool Occupied() => nextSlot >= placePoint.Length;
}
