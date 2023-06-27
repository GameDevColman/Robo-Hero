using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SceneManagerScript : MonoBehaviour
{
    // Declare any public variables that you want to be able 
    // to access throughout your scene
    public ThirdPersonController thirdPersonController;
    // public FirstPersonController firstPersonController;
    // public DialogueManagerScript dialogueManagerScript;
    public InventoryManagerScript inventoryManagerScript;

    public static SceneManagerScript Instance { get; private set; } // static singleton
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        // dialogueManagerScript = FindObjectOfType<DialogueManagerScript>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
        // firstPersonController = FindObjectOfType<FirstPersonController>();
        inventoryManagerScript = FindObjectOfType<InventoryManagerScript>();
    }
}