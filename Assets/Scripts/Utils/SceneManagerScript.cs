using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SceneManagerScript : MonoBehaviour
{
    // Declare any public variables that you want to be able 
    // to access throughout your scene
    public PlayerScript playerScript;
    public DialogManagerScript dialogManagerScript;
    public InventoryManagerScript inventoryManagerScript;
    public StateManagerScript stateManagerScript;

    public static SceneManagerScript Instance { get; private set; } // static singleton
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
        dialogManagerScript = FindObjectOfType<DialogManagerScript>();
        playerScript = FindObjectOfType<PlayerScript>();
        inventoryManagerScript = FindObjectOfType<InventoryManagerScript>();
        stateManagerScript = FindObjectOfType<StateManagerScript>();
    }
}