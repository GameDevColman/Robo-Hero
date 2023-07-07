using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemySteeringScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private int stoppingDistance = 2;
    [SerializeField] private int minDistance = 2;

    public AIState currentState;

    private Animator animator;
    private Rigidbody rigidBody;
    private bool playerHit = false;
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
            case AIState.Pursuit:
                Pursuit();
                break;
            case AIState.Seek:
                Seek();
                break;
            default:
                break;
        }

        animator.SetBool("playerHit", playerHit);
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
            Vector3 moveVector = direction.normalized * moveSpeed;
            moveVector.y = rigidBody.velocity.y;
            rigidBody.velocity = moveVector;
        }
        else
        {
            playerHit = true;
            //enemyHealth.AttackPlayer();
        }

    }

    private void Seek()
    { 
        Vector3 direction = target.position - transform.position;

        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        if (direction.magnitude > minDistance)
        {
            Vector3 moveVector = direction.normalized * moveSpeed * Time.deltaTime;
            transform.position += moveVector;
        }
    }

}
