using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Items
{
    public class AmmoBox : MonoBehaviour
    {
        public Dialog dialog;
        private const int BulletsQuantity = 333;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                SceneManagerScript.Instance.dialogManagerScript.StartDialog(dialog);
                StateManagerScript.Instance.AddBullets(BulletsQuantity);
                Destroy(gameObject);
            }
        }
    }
}
