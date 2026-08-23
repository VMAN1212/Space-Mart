using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public GameObject bulletSpawn;
    public GameObject bullet;
    
    public void onShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        }
    }
}
