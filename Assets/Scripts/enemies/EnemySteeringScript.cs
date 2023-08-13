using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

using CharacterState;

public class EnemySteeringScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private int stoppingDistance = 15;
    [SerializeField] private int attackingDistance = 30;

    public AIState currentState;

    private Animator _animator;
    private Rigidbody _rigidBody;
    private EnemyInventory _enemyinventory;

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
        //enemyHealth = GetComponent<EnemyInventoryScript>();
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Wander"))
        {
            _animator.SetBool("ReadyToShoot", false);
            _animator.SetBool("PlayerInRadius", false);
            currentState = AIState.Seek;
        }
        switch(currentState)
        {
            case AIState.Pursuit:
                Pursuit();
                break;
            case AIState.Seek:
                Seek();
                break;
            case AIState.Attack:
                Attack();
                break;
            case AIState.Idle:
                Idle();
                break;
            default:
                break;
        }
    }

    private void Idle()
    {
        int iterationAhead = 30;
        Vector3 targetSpeed = target.gameObject.GetComponent<FirstPersonController>().instantVelocity;
        Vector3 targetFuturePosition = target.transform.position + (targetSpeed * iterationAhead);
        Vector3 direction = targetFuturePosition - transform.position;
        if (direction.magnitude > attackingDistance)
        {
            currentState = AIState.Seek;
        }
        else if (direction.magnitude > stoppingDistance)
        {
            currentState = AIState.Pursuit;
        }
    }

    // State for pursuiting/chasing the player
    private void Pursuit()
    {
        Debug.Log("Pursuit");
        int iterationAhead = 30;
        Vector3 targetSpeed = target.gameObject.GetComponent<FirstPersonController>().instantVelocity;
        Vector3 targetFuturePosition = target.transform.position + (targetSpeed * iterationAhead);
        Vector3 direction = targetFuturePosition - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        if (direction.magnitude > attackingDistance)
        {
            currentState = AIState.Seek;
        }
        else if (direction.magnitude > stoppingDistance)
        {
            Vector3 moveVector = direction.normalized * moveSpeed;
            moveVector.y = _rigidBody.velocity.y;
            transform.position += moveVector;
            //_rigidBody.velocity = moveVector;
        }
        else
        {
            _animator.SetBool("ReadyToShoot", true);
            currentState = AIState.Idle;
        }
        

    }

    

    // State for seeking the player
    private void Seek()
    { 
        Debug.Log("Seek");
        Vector3 direction = target.position - transform.position;

        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        if (direction.magnitude > attackingDistance)
        {
            Vector3 moveVector = direction.normalized * moveSpeed * Time.deltaTime;
            transform.position += moveVector;
        } else {
            _animator.SetBool("PlayerInRadius", true);
            currentState = AIState.Pursuit;
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

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
        _enemyinventory.health -= damage;
        _enemyinventory.healthBar.SetHealth( _enemyinventory.health);

        // if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f); else animator.SetTrigger("damage");
        if (_enemyinventory.health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        // animator.SetTrigger("death");
        Destroy(gameObject);
    }
}
