using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

public class InventoryManagerScript : MonoBehaviour
{
    public int space = 8;
    public List<Item> items = new List<Item>();
    public Transform inventory;
    
    // Delegate is event that you can subscribe methods to, the event will trigger all methods
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
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
