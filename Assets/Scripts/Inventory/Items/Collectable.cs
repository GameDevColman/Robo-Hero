using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.Serialization;

public class Collectable : MonoBehaviour
{
    public Item item;
    public Dialog dialog;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SceneManagerScript.Instance.dialogManagerScript.StartDialog(dialog);
            bool isSuccessfullyCollected = SceneManagerScript.Instance.inventoryManagerScript.Add(item);
            if (isSuccessfullyCollected)
                Destroy(gameObject);
        }
    }
}
