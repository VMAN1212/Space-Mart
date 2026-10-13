using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Gun gun;
    private bool isHoldingShoot = false;

   public void OnShoot(InputAction.CallbackContext context)
    {
        isHoldingShoot = true;
    }

    public void OnShootRelease(InputAction.CallbackContext context)
    {
        isHoldingShoot = false;
    }


    public void OnReload(InputAction.CallbackContext context)
    {
        if (gun != null)
        {
            gun.TryReload();
        }
    }
    void Update()
    {
        if (isHoldingShoot && gun != null)
        {
            gun.Shoot();
        }
    }
}
