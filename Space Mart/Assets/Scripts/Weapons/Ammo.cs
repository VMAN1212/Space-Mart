using UnityEngine;
using System;

public class Ammo : MonoBehaviour
{
    public int magazineSize = 12;
    public int currentAmmo;
    public int reserveAmmo = 36;
    public int maxReserveAmmo = 120;

    public event Action<int, int> onAmmoChange;

   

    public bool hasAmmo()
    {
        return currentAmmo > 0;
    }

    public bool consumeAmmo(int amount = 1)
    {
        if (currentAmmo < amount) return false;
        currentAmmo -= amount;
        onAmmoChange?.Invoke(currentAmmo, reserveAmmo);
        return true;
    }

    public bool reload()
    {
        if (currentAmmo >= magazineSize) return false;

        int neededAmmo = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        onAmmoChange?.Invoke(currentAmmo, reserveAmmo);
        return true;
    }

    public void addReserveAmmo(int amount)
    {
        reserveAmmo += amount;
        if (reserveAmmo > maxReserveAmmo) reserveAmmo = maxReserveAmmo;
        onAmmoChange?.Invoke(currentAmmo, reserveAmmo);
    }

    public bool isReserveFull()
    {
        return reserveAmmo >= maxReserveAmmo;
    }
}