using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        InitSceneScripts();
        inventoryManagerScript = FindObjectOfType<InventoryManagerScript>();
        stateManagerScript = FindObjectOfType<StateManagerScript>();
    }

    public void InitSceneScripts()
    {
        playerScript = FindObjectOfType<PlayerScript>();
    }

    public static void DestroyAndLoadScene(int scene)
    {
        foreach (GameObject o in FindObjectsOfType<GameObject>()) {
            Destroy(o);
        }
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        Cursor.visible = true;
        // Destroy(GameObject.Find("Canvas"));
        // Destroy(GameObject.Find("Managers"));
    }
}