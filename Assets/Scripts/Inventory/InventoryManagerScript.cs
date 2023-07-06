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

    public static InventoryManagerScript Instance { get; private set; } // static singleton

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public bool Add(Item item)
    {
        if (items.Count >= space)
            return false;

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
        items.Find(i => i.isUsed).isUsed = false;
        items.Find(i => i == item).isUsed = true;
        if (onItemUsedCallback != null)
            onItemUsedCallback.Invoke(item);
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
        }
    }
}
