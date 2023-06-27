using System.Collections;
using System.Collections.Generic;
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
        m_inventorySlots = inventorySlots.GetComponentsInChildren<InventorySlotScript>();
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
}
