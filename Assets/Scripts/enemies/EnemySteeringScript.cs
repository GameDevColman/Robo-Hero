using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using CharacterState;

public class EnemySteeringScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private int attackingDistance = 9;
    [SerializeField] private int pursuitDistance = 15;

    public AIState currentState;
    public EnemyInventory _enemyinventory;

    private Animator _animator;
    private Rigidbody _rigidBody;

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

    private void Idle()
    {
        int iterationAhead = 30;
        Vector3 targetSpeed = target.gameObject.GetComponent<FirstPersonController>().instantVelocity;
        Vector3 targetFuturePosition = target.transform.position + (targetSpeed * iterationAhead);
        Vector3 direction = targetFuturePosition - transform.position;
        if (direction.magnitude > pursuitDistance)
        {
            currentState = AIState.Seek;
        }
        else if (direction.magnitude > attackingDistance)
        {
            currentState = AIState.Pursuit;
        }
    }

    // State for pursuiting/chasing the player
    private void Pursuit()
    {
        Debug.Log("Pursuit");
        int iterationAhead = 2;
        Vector3 targetSpeed = target.gameObject.GetComponent<FirstPersonController>().instantVelocity;
        Vector3 targetFuturePosition = target.transform.position + (targetSpeed * iterationAhead);
        Vector3 direction = targetFuturePosition - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        // Enemy is close enougth to attack
        if (direction.magnitude < attackingDistance)
        {
            _animator.SetBool("ReadyToShoot", true);
            currentState = AIState.Attack;
        }
        // Enemy is still at pursuit distance
        else if (direction.magnitude < pursuitDistance)
        {
            Vector3 moveVector = direction.normalized * moveSpeed;
            moveVector.y = _rigidBody.velocity.y;
            transform.position += moveVector;
            _rigidBody.velocity = moveVector;
        }
        // Enemy is ar away, seek
        else
        {
            currentState = AIState.Seek;
        }
    }

    

    // State for seeking the player
    private void Seek()
    { 
        Debug.Log("Seek");
        _animator.SetBool("ReadyToShoot", false);
        _animator.SetBool("PlayerInRadius", false);
        Vector3 direction = target.position - transform.position;

        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, 
        Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        if (direction.magnitude > pursuitDistance)
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
        Debug.Log("Attack");
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            var cannonPos = GameObject.FindGameObjectWithTag("EnemyCannon").transform.position;
            // Rigidbody rb = Instantiate(projectile, cannonPos, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(player.transform.position - cannonPos, ForceMode.Impulse);
            // rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            _animator.SetBool("ReadyToShoot", true);
            // Vector3 cannonPos = GameObject.FindGameObjectWithTag("EnemyCannon").transform.position;
            // GameObject newProjectile = Instantiate(projectile, cannonPos, Quaternion.identity);
        
            Vector3 shootDirection = (player.transform.position - cannonPos).normalized;
            Rigidbody rb = Instantiate(projectile, cannonPos, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(shootDirection * 2f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        Vector3 direction = target.position - transform.position;
        int rndMistake = Random.Range(1, 5);
        // While player escaping, enemy gets tired
        if (direction.magnitude > attackingDistance - rndMistake)
        {
            _animator.SetBool("ReadyToShoot", false);
            currentState = AIState.Seek;
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        _enemyinventory.health -= damage;
        _enemyinventory.healthBar.SetHealth(_enemyinventory.health);

        if (_enemyinventory.health <= 0) Invoke(nameof(DestroyEnemy), 0.5f); 
        else _animator.SetBool("playerHit", true);
    }
    private void DestroyEnemy()
    {
        // animator.SetTrigger("death");
        Destroy(gameObject);
    }
}
