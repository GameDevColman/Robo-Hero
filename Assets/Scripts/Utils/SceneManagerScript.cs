using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SceneManagerScript : MonoBehaviour
{
    // Declare any public variables that you want to be able 
    // to access throughout your scene
    public PlayerScript playerScript;
    // public FirstPersonController firstPersonController;
    public DialogManagerScript dialogManagerScript;
    public InventoryManagerScript inventoryManagerScript;

    public static SceneManagerScript Instance { get; private set; } // static singleton
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        dialogManagerScript = FindObjectOfType<DialogManagerScript>();
        playerScript = FindObjectOfType<PlayerScript>();
        // firstPersonController = FindObjectOfType<FirstPersonController>();
        inventoryManagerScript = InventoryManagerScript.Instance;
    }
}