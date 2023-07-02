using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Items
{
    public class AmmoBox : MonoBehaviour
    {
        public Text quantity;
        public Dialog dialog;
        private const int BulletsQuantity = 333;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                SceneManagerScript.Instance.dialogManagerScript.StartDialog(dialog);
                quantity.text = (int.Parse(quantity.text) + BulletsQuantity).ToString();
                Destroy(gameObject);
            }
        }
    }
}
