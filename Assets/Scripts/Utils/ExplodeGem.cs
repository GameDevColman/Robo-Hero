using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;
using Random = UnityEngine.Random;

public class ExplodeGem : MonoBehaviour
{
    public float destroyDelay;
    public float minForce;
    public float maxForce;
    public float radius;
    public AudioClip collectionSound;
    
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
                rb.AddExplosionForce(Random.Range(minForce, maxForce), tPos, radius);
                Destroy(t.gameObject, destroyDelay);
            }
        }
    }
}
