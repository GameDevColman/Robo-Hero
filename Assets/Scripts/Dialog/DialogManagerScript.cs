using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogManagerScript : MonoBehaviour
{
    public Animator animator;
    public Text dialogText;
    public Image leftCharacterImage;
    public Image rightCharacterImage;

    private Queue<DialogElement> sentences;
    private UnityEvent afterDialogAction;
    private AudioSource audioSource;

    void Start()
    {
        sentences = new Queue<DialogElement>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void StartDialog(Dialog dialog)
    {
        afterDialogAction = dialog.afterDialogAction;
        animator.SetBool("isOpen", true);
        sentences.Clear();
        SetPlayerIsInDialogTrue();
        foreach (DialogElement dialogElement in dialog.sentences)
        {
            sentences.Enqueue(dialogElement);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        StopAllCoroutines();
        DialogElement dialogElement = sentences.Dequeue();
        PlaceCharacterImage(dialogElement.characterImage, dialogElement.placeImageToTheRight);
        dialogText.color = dialogElement.sentenceColor;
        StartCoroutine(TypeSentence(dialogElement.sentence, dialogElement.sentenceAudio));
        dialogElement.afterDialogAction.Invoke();
    }

    private IEnumerator TypeSentence(string sentence, AudioClip sentenceAudio)
    {
        if (sentenceAudio != null)
        {
            audioSource.Stop();
            audioSource.clip = sentenceAudio;
            audioSource.Play();
        }
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        { 
            dialogText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void EndDialog()
    {
        rightCharacterImage.enabled = false;
        leftCharacterImage.enabled = false;
        animator.SetBool("isOpen", false);
        SetPlayerIsInDialogFalse();
        afterDialogAction.Invoke();
    }

    private void PlaceCharacterImage(Sprite sprite, bool placeImageToTheRight)
    {
        rightCharacterImage.enabled = false;
        leftCharacterImage.enabled = false;
        if (sprite)
        {
            if (placeImageToTheRight)
            {
                rightCharacterImage.sprite = sprite;
                rightCharacterImage.enabled = true;
            }
            else
            {
                leftCharacterImage.sprite = sprite;
                leftCharacterImage.enabled = true;
            }
        }
    }

    private void SetPlayerIsInDialogTrue()
    {
        if (SceneManagerScript.Instance.playerScript != null)
            SceneManagerScript.Instance.playerScript.isInDialog = true;
        // if (SceneManagerScript.Instance.thirdPersonController != null)
        //     SceneManagerScript.Instance.thirdPersonController.isInDialogue = true;
    }

    private void SetPlayerIsInDialogFalse()
    {
        if (SceneManagerScript.Instance.playerScript != null)
            SceneManagerScript.Instance.playerScript.isInDialog = false;
        // if (SceneManagerScript.Instance.thirdPersonController != null)
        //     SceneManagerScript.Instance.thirdPersonController.isInDialogue = false;
    }
}
