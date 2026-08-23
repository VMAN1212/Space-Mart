using UnityEngine;
using TMPro;

public class ShelfUI : MonoBehaviour
{
    [SerializeField] public int requiredItems;
    [SerializeField] public TextMeshProUGUI counter;
    private int placedItems;
    public static ShelfUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        updateUI();
    }

    public void Placed()
    {
        placedItems++;
        updateUI();

        if(placedItems == requiredItems)
        {
            taskCompleted();
        }
    }

    private void updateUI()
    {
        counter.text = placedItems + "/" + requiredItems + " items placed.";
    }

    private void taskCompleted()
    {
        counter.text = "Task Completed.";
    }
}
