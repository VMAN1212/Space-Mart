using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public Transform bulletSpawn;
    public Ammo ammo;
    public float reloadTime = 0.5f;

    [Header("Tutorial Hook")]
    [SerializeField] private bool trackShootingForTutorial = true;
    private bool hasAdvancedShootingTutorial = false;

    private bool isReloading = false;

    private void Awake()
    {
        if (ammo == null)
        {
            ammo = GetComponent<Ammo>();
        }
    }

    public void onShoot(InputAction.CallbackContext context)
    {
        if (!context.performed || isReloading) return;

        if (ammo != null && !ammo.consumeAmmo(1))
        {
            return;
        }

        BulletPool.Instance.GetBullet(bulletSpawn.position, bulletSpawn.rotation);

       
        if (trackShootingForTutorial && !hasAdvancedShootingTutorial)
        {
            hasAdvancedShootingTutorial = true;

            if (TutorialManager.Instance != null)
            {
                TutorialManager.Instance.ProgressTutorial();
            }
        }
    }

    public void onReload(InputAction.CallbackContext context)
    {
        if (!context.performed || isReloading || ammo == null) return;

        if (ammo.currentAmmo >= ammo.magazineSize) return;

        if (ammo.reserveAmmo > 0)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private System.Collections.IEnumerator ReloadRoutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        ammo.reload();
        isReloading = false;
    }
}