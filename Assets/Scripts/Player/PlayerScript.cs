using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerScript : MonoBehaviour
{
    public bool isInDialog;
    public Camera playerCamera;
    public Dialog dialog;
    public UnityEvent actions = new UnityEvent();

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Cursor.visible = false;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInDialog)
            {
                SceneManagerScript.Instance.dialogManagerScript.DisplayNextSentence();
            }
        }
    }

    public void KillPlayer()
    {
        actions.Invoke();
        Destroy(GameObject.Find("GunContainer"));
        Destroy(gameObject.GetComponent<FirstPersonController>());
        Destroy(gameObject.GetComponent<CharacterController>());
    }
    
    public void StartDialog()
    {
        SceneManagerScript.Instance.dialogManagerScript.StartDialog(dialog);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        SceneManagerScript.Instance.stateManagerScript.TakeDamage();
    }
}
