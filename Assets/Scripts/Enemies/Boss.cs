using System.Collections;
using CharacterState;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public Dialog dialog;
    public UnityEvent actions = new UnityEvent();
    public NavMeshAgent agent;

    public Transform player;
    public HealthBar healthBar;

    // public Animator animator;

    public LayerMask whatIsPlayer;

    public int health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    private bool isWaiting = false;
    public Transform wayPoints;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    
    public void StartDialog()
    {
        SceneManagerScript.Instance.dialogManagerScript.StartDialog(dialog);
    }

    public void EndScene()
    {
        // Todo: add end scene logic
    }

    private void Update()
    {
        if (!isWaiting)
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
    
        if (walkPointSet) 
            agent.SetDestination(walkPoint);
    
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
    
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            StartCoroutine(WaitTime());
        }
    }
    
    IEnumerator WaitTime() {
        isWaiting = true;
        yield return new WaitForSeconds(2f);
        isWaiting = false;
    }
    
    private void SearchWalkPoint()
    {
        walkPoint = wayPoints.GetChild(Random.Range(0, wayPoints.childCount)).transform.position;
        walkPointSet = true;
    }
    
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
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
        if (health <= 0) DestroyEnemy();
    }
    private void DestroyEnemy()
    {
        // animator.SetTrigger("death");
        actions.Invoke();
        Destroy(gameObject);
        SceneManagerScript.DestroyAndLoadScene(5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
