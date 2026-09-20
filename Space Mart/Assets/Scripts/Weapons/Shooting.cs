using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public GameObject bulletSpawn;
    public GameObject bullet;
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

            Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        }
    }
}
