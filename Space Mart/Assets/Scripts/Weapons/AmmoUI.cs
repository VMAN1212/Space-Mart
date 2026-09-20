using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public Ammo ammo;
    public TextMeshProUGUI ammoCount;

    private void Update()
    {
        if (ammo == null || ammoCount == null) return;

        ammoCount.text = ammo.currentAmmo + " / " + ammo.maxAmmo;
    }
}
