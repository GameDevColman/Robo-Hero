using System;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        // Using `new` is overriding the default `name` property for object
        new public string name = "New Item";
        public Sprite icon = null;
        public bool isUsed = false;

        public virtual void ChangeUsage()
        {
            isUsed = !isUsed;
            if (isUsed) Debug.Log("using item: " + name);
        }
    }
}
