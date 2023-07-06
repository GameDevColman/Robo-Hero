using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public Dialog dialog;
    public UnityEvent actions = new UnityEvent();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            actions.Invoke();
            Destroy(gameObject.GetComponent<Collider>());
        }
    }
    
    public void StartDialog()
    {
        SceneManagerScript.Instance.dialogManagerScript.StartDialog(dialog);
    }

    public void EndScene(int sceneIndex)
    {
        StartCoroutine(MoveToEndSceneAfterDelay(2f, sceneIndex));
    }

    IEnumerator MoveToEndSceneAfterDelay(float delay, int sceneIndex)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}
