using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterState;

public class EnemyInventory : MonoBehaviour
{
    public HealthBar healthBar;
    public int health;
    
     //Attacking
    public Transform player;
    public UnityEngine.AI.NavMeshAgent agent;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Attack()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            foreach (GameObject cannon in GameObject.FindGameObjectsWithTag("Cannon"))
            {
                var cannonPos = cannon.transform.position;
                Rigidbody rb = Instantiate(projectile, cannonPos, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(player.transform.position - cannonPos, ForceMode.Impulse);
            }

            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        // if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f); else animator.SetTrigger("damage");
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        // animator.SetTrigger("death");
        Destroy(gameObject);
    }
}
