using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ran.Item;

public class InventoryManagerScript : MonoBehaviour
{
    public int space = 4;
    public List<Item> items = new List<Item>();

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

    }
}
