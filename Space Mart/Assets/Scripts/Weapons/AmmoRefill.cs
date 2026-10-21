using UnityEngine;

public class AmmoRefill : MonoBehaviour, IInteractable
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip ammoSound;

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
        if (ammo.isReserveFull()) return;

        ammo.addReserveAmmo(ammoAmount);
        lastPickup = Time.time;

        if (audioSource != null && ammoSound != null)
        {
            audioSource.PlayOneShot(ammoSound);
        }
    }
}