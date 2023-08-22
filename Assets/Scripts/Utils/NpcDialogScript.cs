using UnityEngine;

public class NpcDialogScript : MonoBehaviour
{
    public Dialog dialog;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartDialog();
            Destroy(gameObject);
        }
    }

    public void StartDialog()
    {
        SceneManagerScript.Instance.dialogManagerScript.StartDialog(dialog);
    }
}
