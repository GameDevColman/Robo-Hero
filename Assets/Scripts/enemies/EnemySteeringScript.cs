using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

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

    private void Pursuit()
    {
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

    private void Seek()
    { 
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
}
