using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectLife : MonoBehaviour
{
    public AudioClip collectionSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collider");
            AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        }
    }
}
