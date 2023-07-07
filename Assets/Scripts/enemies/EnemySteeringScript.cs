using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemySteeringScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private int stoppingDistance = 2;
    [SerializeField] private int safeDistance = 100;

    public AIState currentState;

    private Animator animator;
    private Rigidbody rigidBody;
    private bool isWalking = false;
    //private EnemyHealthScript enemyHealth;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        //enemyHealth = GetComponent<EnemyHealthScript>();
    }

    private void Update()
    {
        switch(currentState)
        {
            case AIState.Idle:
                break;
            case AIState.Pursuit:
                Pursuit();
                break;
            case AIState.Evade:
                Evade();
                break;
            default:
                break;
        }

        animator.SetBool("isWalking", isWalking);
    }

    private void Pursuit()
    {
        int iterationAhead = 30;
        Vector3 targetSpeed = target.gameObject.GetComponent<FirstPersonController>().instantVelocity;
        Vector3 targetFuturePosition = target.transform.position + (targetSpeed * iterationAhead);
        Vector3 direction = targetFuturePosition - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        if (direction.magnitude > stoppingDistance)
        {
            isWalking = true;
            Vector3 moveVector = direction.normalized * moveSpeed;
            moveVector.y = rigidBody.velocity.y;
            rigidBody.velocity = moveVector;
        }
        else
        {
            isWalking = false;
            //enemyHealth.AttackPlayer();
        }

    }

    private void Evade()
    {
        int iterationAhead = 30;
        Vector3 targetSpeed = target.gameObject.GetComponent<FirstPersonController>().instantVelocity;
        Vector3 targetFuturePosition = target.transform.position + (targetSpeed * iterationAhead);
        Vector3 direction = transform.position - targetFuturePosition;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        if (direction.magnitude < safeDistance)
        {
            isWalking = true;
            Vector3 moveVector = direction.normalized * moveSpeed;
            moveVector.y = rigidBody.velocity.y;
            rigidBody.velocity = moveVector;
        }
        else
            isWalking = false;
    }

}
