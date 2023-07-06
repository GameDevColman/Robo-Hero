using Inventory.Items;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUIScript : MonoBehaviour
{
    public Transform inventorySlots;

    private InventoryManagerScript m_inventoryManagerScript;
    private InventorySlotScript[] m_inventorySlots;

    void Start()
    {
        m_inventoryManagerScript = SceneManagerScript.Instance.inventoryManagerScript;
        m_inventoryManagerScript.onItemChangedCallback += UpdateUI;
        m_inventoryManagerScript.onItemUsedCallback += UpdateSlot;
        m_inventorySlots = inventorySlots.GetComponentsInChildren<InventorySlotScript>();
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < m_inventorySlots.Length; i++)
        {
            if (i < m_inventoryManagerScript.items.Count)
            {
                m_inventorySlots[i].AddItem(m_inventoryManagerScript.items[i]);
            }
            else
            {
                m_inventorySlots[i].ClearSlot();
            }    
        }
    }
    
    void UpdateSlot(Item item)
    {
        for (int i = 0; i < m_inventorySlots.Length; i++)
        {
            m_inventorySlots[i].UnuseItem();
            if (item == m_inventorySlots[i].item)
            {
                m_inventorySlots[i].UseItem();
            }
        }
    }
}
