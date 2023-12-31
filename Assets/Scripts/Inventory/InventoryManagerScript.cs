using System;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

public class InventoryManagerScript : MonoBehaviour
{
    public int space = 4;
    public List<Item> items = new List<Item>();
    public Transform inventory;
    
    // Delegate is event that you can subscribe methods to, the event will trigger all methods
    public delegate void OnItemChanged();
    public delegate void OnItemUsed(Item item);
    public OnItemChanged onItemChangedCallback;
    public OnItemUsed onItemUsedCallback;
    
    private const int ALPHA_KEY_OFFSET = 49;

    public bool Add(Item item)
    {
        if (items.Count >= space)
            return false;

        item.isUsed = false;
        items.Add(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
    
    public void Use(Item item)
    {
        var usedItem = items.Find(i => i.isUsed && i != item);
        if (usedItem != null)
        {
            usedItem.isUsed = false;
            Unuse(usedItem);
        }

        if (onItemUsedCallback != null)
        {
            ChangeItemUsage(item, true);
        }
    }

    private void ChangeItemUsage(Item item, bool isUsed)
    {
        GameObject.Find("GunContainer").transform.Find(item.name).gameObject.SetActive(isUsed);
        GameObject.Find("AimCanvas").transform.Find("Aim").gameObject.SetActive(isUsed);
        onItemUsedCallback.Invoke(item);
    }

    public void Unuse(Item item)
    {
        if (onItemUsedCallback != null && item != null)
        {
            ChangeItemUsage(item, false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
        }

        if (SceneManagerScript.Instance.IsUseEnabled()) InitInventorySlotsKeyDown();
    }
    
    private void InitInventorySlotsKeyDown()
    {
        for (int i = 0; i < items.Count; i++)
        {
            HandleInventorySlotKeyDown((KeyCode)(i + ALPHA_KEY_OFFSET));
        }
    }
    
    private void HandleInventorySlotKeyDown(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            int inventorySlotPressed = ((int)keyCode) - ALPHA_KEY_OFFSET;
            items[inventorySlotPressed].ChangeUsage();
        }
    }

    public void RemoveUsedItem()
    {
        foreach (Item item in items)
        {
            if (item.isUsed) item.ChangeUsage();
        }
    }
}
