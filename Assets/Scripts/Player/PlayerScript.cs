using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
    private const int ALPHA_KEY_OFFSET = 49;

    public bool isInDialog;
    public Camera playerCamera;
    // public StealthStatScript stealthStatScript;
    // public PlayerAttachedVolumeScript playerAttachedVolumeScript;

    private InventoryManagerScript m_inventoryManagerScript;
    private StateManagerScript m_stateManagerScript;

    private void Start()
    {
        Cursor.visible = false;
        m_inventoryManagerScript = SceneManagerScript.Instance.inventoryManagerScript != null ?
            SceneManagerScript.Instance.inventoryManagerScript : InventoryManagerScript.Instance;
        m_stateManagerScript = SceneManagerScript.Instance.stateManagerScript != null ?
            SceneManagerScript.Instance.stateManagerScript : StateManagerScript.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInDialog)
            {
                SceneManagerScript.Instance.dialogManagerScript.DisplayNextSentence();
            }
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

    public void KillPlayer()
    {
        // Todo: add kill logic
        SceneManager.LoadScene(2);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        m_stateManagerScript.TakeDamage();
    }
}
