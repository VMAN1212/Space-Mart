using UnityEngine;
using System;
using JetBrains.Annotations;

public class Ammo : MonoBehaviour
{
    public int maxAmmo = 24;
    public int currentAmmo;
    public event Action<int, int> onAmmoChange;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public bool hasAmmo()
    {
        return currentAmmo > 0;
    }

    public bool consumeAmmo(int amount = 1)
    {
        if (currentAmmo < amount) return false;
        currentAmmo -= amount;
        onAmmoChange?.Invoke(currentAmmo, maxAmmo);
        return true;
    }

    public void addAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        onAmmoChange?.Invoke(currentAmmo, maxAmmo);
    }

    public bool isFull()
    {
        return currentAmmo >= maxAmmo;
    }
}
