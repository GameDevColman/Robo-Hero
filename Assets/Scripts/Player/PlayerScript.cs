using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private const int ALPHA_KEY_OFFSET = 49;

    public bool isInDialog;
    public Camera playerCamera;
    public GameObject projectile;
    public Transform leftFirePoint, rightFirePoint;
    public float projectileSpeed = 30f;
    public float fireRate = 2f;
    public float arcRange = 1f;
    public float attackDamage = 50f;
    public bool hasProjectileAbility = true;
    public bool hasEquipAbility = true;
    // public StealthStatScript stealthStatScript;
    // public PlayerAttachedVolumeScript playerAttachedVolumeScript;

    private InventoryManagerScript m_inventoryManagerScript;
    private Vector3 m_projectileDestination;
    private bool m_isLeftHand;
    private float m_timeToFire;

    private void Start()
    {
        Cursor.visible = false;
        m_inventoryManagerScript = SceneManagerScript.Instance.inventoryManagerScript != null ?
            SceneManagerScript.Instance.inventoryManagerScript : InventoryManagerScript.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInDialog)
            {
                SceneManagerScript.Instance.dialogManagerScript.DisplayNextSentence();
            }
            else
            {
                // CheckInteraction();
            }
        }

        if (Input.GetKeyDown(KeyCode.C) && hasProjectileAbility && Time.time >= m_timeToFire)
        {
            m_timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }

        initInventorySlotsKeyDown();
    }

    private void initInventorySlotsKeyDown()
    {
        for (int i = 0; i < m_inventoryManagerScript.items.Count; i++)
        {
            handleInventorySlotKeyDown((KeyCode)(i + ALPHA_KEY_OFFSET));
        }
    }

    private void handleInventorySlotKeyDown(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            int inventorySlotPressed = ((int)keyCode) - ALPHA_KEY_OFFSET;
            m_inventoryManagerScript.items[inventorySlotPressed].Use() ;
        }
    }

    private void ShootProjectile()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        m_projectileDestination = ray.GetPoint(1000);
        if (m_isLeftHand)
        {
            m_isLeftHand = false;
            InstantiateProjectile(leftFirePoint);
        }
        else
        {
            m_isLeftHand = true;
            InstantiateProjectile(rightFirePoint);
        }
    }

    private void InstantiateProjectile(Transform firePoint)
    {
        GameObject projectileObject = Instantiate(projectile, firePoint.position, Quaternion.identity);
        projectileObject.GetComponent<Rigidbody>().velocity = (m_projectileDestination - firePoint.position).normalized * projectileSpeed;
        iTween.PunchPosition(projectileObject, new Vector3(Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0), Random.Range(0.5f, 1f));
    }

    public void TurnOnProjectileAbility()
    {
        hasProjectileAbility = true;
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(2);
    }

    // public void TakeStealthDamage(float damage)
    // {
    //     stealthStatScript.TakeStealthDamage(damage);
    //     if (stealthStatScript.currentStealth <= 0)
    //         KillPlayer();
    // }

    // public void WarnPlayer()
    // {
    //     playerAttachedVolumeScript.EnableVolume();
    // }
    //
    // public void ClearWarnPlayer()
    // {
    //     playerAttachedVolumeScript.DisableVolume();
    // }
    //
    // void CheckInteraction()
    // {
    //     Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
    //     if (hits.Length > 0)
    //     {
    //         foreach (Collider hit in hits)
    //         {
    //             if ((hit.transform.GetComponent<InteractableScript>()) &&
    //                 (hit.transform.GetComponent<InteractableScript>().isInteractable))
    //             {
    //                 hit.transform.GetComponent<InteractableScript>().Interact();
    //                 return;
    //             }
    //         }
    //     }
    // }
}
