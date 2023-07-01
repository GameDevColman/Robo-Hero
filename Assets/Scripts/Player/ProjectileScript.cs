using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject impactVfx;

    private bool m_isCollided;

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag != "Bullet") && (collision.gameObject.tag != "Player") && !m_isCollided)
        {
            m_isCollided = true;
            GameObject impact = Instantiate(impactVfx, collision.contacts[0].point, Quaternion.identity);
            Destroy(impact, 2f);
            Destroy(gameObject);
        }
    }
}
