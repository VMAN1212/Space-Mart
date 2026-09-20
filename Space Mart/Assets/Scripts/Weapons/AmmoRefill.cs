using UnityEngine;
using UnityEngine.InputSystem;

public class AmmoRefill : MonoBehaviour, IInteractable
{
    public int ammoAmount = 12;
    public float cooldown = 5f;
    private float lastPickup = -999f;
    public Ammo ammo;

    public string getPrompt()
    {
        return "pick up eggs.";
    }

    public void Interact()
    {
        if (ammo == null) return;
        if (Time.time - lastPickup < cooldown) return;
        if (ammo.isFull()) return;

        ammo.addAmmo(ammoAmount);
        lastPickup = Time.time;
    }
}