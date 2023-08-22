using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ArmamentItem")]
    public class ArmamentItem : Item
    {
        public GameObject armamentPrefab;
        public AudioClip successUsingItemAudio;

        public override void Use()
        {
            if (!isUsed)
            {
                Transform playerTransform = SceneManagerScript.Instance.playerScript.transform;
                // Instantiate(armamentPrefab, playerTransform.position + playerTransform.TransformDirection(new Vector3(0, -0.5f, 1f)), playerTransform.rotation);
                SceneManagerScript.Instance.inventoryManagerScript.Use(this);
                if (successUsingItemAudio) 
                    AudioSource.PlayClipAtPoint(successUsingItemAudio, playerTransform.transform.position);
            }
            else
            {
                SceneManagerScript.Instance.inventoryManagerScript.Unuse(this);
            }
            
            base.Use();
        }
    }
}
