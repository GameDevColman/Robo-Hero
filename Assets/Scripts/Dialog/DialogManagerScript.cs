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
    private UnityEvent afterDialogueAction;
    private AudioSource audioSource;

    void Start()
    {
        sentences = new Queue<DialogElement>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void StartDialogue(Dialog dialog)
    {
        afterDialogueAction = dialog.afterDialogAction;
        animator.SetBool("isOpen", true);
        sentences.Clear();
        SetPlayerIsInDialogueTrue();
        foreach (DialogElement dialogueElement in dialog.sentences)
        {
            sentences.Enqueue(dialogueElement);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
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

    private void EndDialogue()
    {
        rightCharacterImage.enabled = false;
        leftCharacterImage.enabled = false;
        animator.SetBool("isOpen", false);
        SetPlayerIsInDialogueFalse();
        afterDialogueAction.Invoke();
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

    private void SetPlayerIsInDialogueTrue()
    {
        // if (SceneManagerScript.Instance.firstPersonController != null)
        //     SceneManagerScript.Instance.firstPersonController.isInDialogue = true;
        // if (SceneManagerScript.Instance.thirdPersonController != null)
        //     SceneManagerScript.Instance.thirdPersonController.isInDialogue = true;
    }

    private void SetPlayerIsInDialogueFalse()
    {
        // if (SceneManagerScript.Instance.firstPersonController != null)
        //     SceneManagerScript.Instance.firstPersonController.isInDialogue = false;
        // if (SceneManagerScript.Instance.thirdPersonController != null)
        //     SceneManagerScript.Instance.thirdPersonController.isInDialogue = false;
    }
}
