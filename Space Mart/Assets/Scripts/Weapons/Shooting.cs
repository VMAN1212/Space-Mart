using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public Transform bulletSpawn;
    public Ammo ammo;

    private void Awake()
    {
        if (ammo == null)
        {
            ammo = GetComponent<Ammo>();
        }
    }

    public void onShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ammo != null && !ammo.consumeAmmo(1))
            {
                return;
            }

            BulletPool.Instance.GetBullet(bulletSpawn.position, bulletSpawn.rotation);
        }
    }
}
