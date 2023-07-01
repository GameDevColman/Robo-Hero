using Inventory.Items;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ArmamentItem")]
public class ArmamentItem : Item
{
    public string areaTagName;
    public GameObject armamentPrefab;
    public AudioClip successUsingItemAudio;

    public override void Use()
    {
        base.Use();
        Collider[] hits = Physics.OverlapSphere(SceneManagerScript.Instance.playerScript.transform.position, 2f);
        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag(areaTagName))
                {
                    Transform playerTransform = SceneManagerScript.Instance.playerScript.transform;
                    Instantiate(armamentPrefab, playerTransform.position + playerTransform.TransformDirection(new Vector3(0, -0.5f, 1f)), playerTransform.rotation);
                    SceneManagerScript.Instance.inventoryManagerScript.Remove(this);
                    if (successUsingItemAudio)
                        AudioSource.PlayClipAtPoint(successUsingItemAudio, hit.transform.position);
                }
            }
        }
    }
}
