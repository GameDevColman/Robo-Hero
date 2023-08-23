using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExplodeGem : MonoBehaviour
{
    // Exploding params
    public float destroyDelay;
    public float minForce;
    public float maxForce;
    public float radius;
    
    // Player
    public Transform player;
    
    // Audio
    public AudioClip collectionSound;
    
    // Manage bullets
    public StateManagerScript stateManagerScript;

    private void Start()
    {
        var stateManagerScript = FindObjectOfType<StateManagerScript>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        foreach (Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 tPos = transform.position + Vector3.forward;
                AudioSource.PlayClipAtPoint(collectionSound, tPos);
                stateManagerScript.AddBullets(10);
                rb.AddExplosionForce(Random.Range(minForce, maxForce), tPos, radius);
                Destroy(t.gameObject, destroyDelay);
            }
        }
    }
}
