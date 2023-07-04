using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    public Image icon;
    public Item item;
    public void AddItem(Item item)
    {
        this.item = item;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void UseItem()
    {
        GetComponent<Image>().color = new Color(0.3686274509803922f, 0, 0.3686274509803922f, 1);
    }

    public void UnuseItem()
    {
        GetComponent<Image>().color = Color.black;
    }
}
