using UnityEngine;

namespace Inventory.Items
{
    public class Collectable : MonoBehaviour
    {
        public Item item;
        public Dialog dialog;

        private void OnCollisionEnter(UnityEngine.Collision collision)
        {
            Debug.Log("Hi");
            if (collision.collider.CompareTag("Player"))
            {
                SceneManagerScript.Instance.dialogManagerScript.StartDialog(dialog);
                bool isSuccessfullyCollected = SceneManagerScript.Instance.inventoryManagerScript.Add(item);
                if (isSuccessfullyCollected)
                    Destroy(gameObject);
            }
        }
    }
}
