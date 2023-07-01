using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public AudioClip boomSound; //mine boom

   private void OnCollisionEnter(Collision collision) 
    {
        PlayerInventory playerInventory = collision.gameObject.GetComponent<PlayerInventory>();

        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(boomSound, transform.position); //plays our soundclip
        }
    }
}
