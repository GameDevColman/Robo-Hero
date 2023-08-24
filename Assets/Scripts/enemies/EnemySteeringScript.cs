using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public class EnemySteeringScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private int attackingDistance = 10;
    [SerializeField] private int pursuitDistance = 20;
    
    private Animator _animator;
    private Rigidbody _rigidBody;
    
    // Manage states
    public AIState currentState;
    public EnemyInventory enemyinventory;

    //Attacking
    public Transform player;
    public UnityEngine.AI.NavMeshAgent agent;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        switch(currentState)
        {
            case AIState.Seek:
                Seek();
                break;
            case AIState.Pursuit:
                Pursuit();
                break;
            case AIState.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    // State for pursuiting/chasing the player
    private void Pursuit()
    {
        _animator.SetBool("PlayerInRadius", true);
        _animator.SetBool("ReadyToShoot", false);

        int iterationAhead = 30;
        Vector3 targetSpeed = target.gameObject.GetComponent<FirstPersonController>().instantVelocity;
        Vector3 targetFuturePosition = target.transform.position + (targetSpeed * iterationAhead);
        Vector3 direction = targetFuturePosition - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(direction), 
            rotationSpeed * Time.deltaTime);
        // Enemy is far away from player
        if (direction.magnitude > pursuitDistance)
        {
            currentState = AIState.Seek;
        }
        // Enemy should pursuit
        else if (direction.magnitude > attackingDistance)
        {
            Vector3 moveVector = direction.normalized * moveSpeed * Time.deltaTime;
            moveVector.y = _rigidBody.velocity.y;
            transform.position += moveVector;
            _rigidBody.velocity = moveVector;
        }
        // Enemy can attack
        else
        {
            currentState = AIState.Attack;
        }
    }

    

    // State for seeking the player
    private void Seek()
    {
        _animator.SetBool("ReadyToShoot", false);
        _animator.SetBool("PlayerInRadius", false);
        Vector3 direction = target.position - transform.position;

        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, 
        Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        if (direction.magnitude > pursuitDistance)
        {
            Vector3 moveVector = direction.normalized * moveSpeed * Time.deltaTime * 2;
            transform.position += moveVector;
        } else {
            currentState = AIState.Pursuit;
        }
    }

    private void Attack()
    {
        _animator.SetBool("ReadyToShoot", true);
        _animator.SetBool("PlayerInRadius", true);
        
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        
        if (!alreadyAttacked)
        {
            var cannonPos = GameObject.FindGameObjectWithTag("EnemyCannon").transform.position;

            Rigidbody rb = Instantiate(projectile, cannonPos, Quaternion.identity).GetComponent<Rigidbody>();
            rb.velocity = transform.TransformDirection(new Vector3(0, 2,0));
            rb.AddForce(player.transform.position - cannonPos, ForceMode.Impulse);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        Vector3 direction = target.position - transform.position;
        int rndMistake = Random.Range(1, 8);
        // While player escaping, enemy gets tired
        if (direction.magnitude + rndMistake > attackingDistance)
        {
            currentState = AIState.Pursuit;
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        enemyinventory.health -= damage;
        enemyinventory.healthBar.SetHealth(enemyinventory.health);

        if (enemyinventory.health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        _animator.SetBool("playerHit", true);
        Destroy(gameObject, 10);
    }
}
