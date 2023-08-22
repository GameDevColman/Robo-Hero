using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public string objectID;

    private void Awake()
    {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    void Start()
    {
        for (int i = 0; i < FindObjectsOfType<DontDestroy>().Length; i++)
        {
            var dontDestroy = FindObjectsOfType<DontDestroy>()[i];
            if (dontDestroy != this && dontDestroy.objectID == objectID)
            {
                Destroy(gameObject);
            }
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManagerScript.Instance.InitSceneScripts();
    }
}
