using UnityEngine;
using System.Collections;

public class PaparazziAI2 : MonoBehaviour
{
    public Animator animator;

    // bool isWalking = false;
    // bool isRunning = false;

    public Transform[] patrolPoints;     // Set in Inspector
    public Transform player;             // Player reference
    public BayesianDecisionManager bayesManager;
    public ParticleController PaparazziParticleController;

    public float chaseThreshold = 0.6f;
    public float viewDistance = 10f;
    public float speed = 0.1f;
    public float runningSpeed = 5f;
    public float rotationSpeed = 10f;

    private int currentPatrolIndex = 0;
    private enum State { Patrol, Chase, Search }
    private State currentState = State.Patrol;

    private bool isReturningToPatrol = false;

    private void Start()
    {
        if (patrolPoints.Length > 0)
            transform.position = patrolPoints[0].position;


        // animator.SetBool("isWalking", true);
        // animator.SetBool("isRunning", false);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool shouldChase = bayesManager.ShouldChase();

        switch (currentState)
        {
            case State.Patrol:
                PatrolLogic();
                
                PaparazziParticleController.stopParticleSystem();
                Debug.Log("Stopping particle System");

                // Probabilistic chase trigger
                if (shouldChase && distanceToPlayer < viewDistance)
                {
                    currentState = State.Chase;
                    Debug.Log("Switching to Chase!");
                }
                break;

            case State.Chase:
                ChasePlayer();

                PaparazziParticleController.playParticleSystem();
                Debug.Log("Playing particle System");

                // Stop chasing if player escapes
                if (distanceToPlayer > viewDistance)
                {
                    currentState = State.Search;
                    Debug.Log("Lost sight of player. Searching...");                    
                }
                break;

            case State.Search:
                if (!isReturningToPatrol)
                {
                    StartCoroutine(ReturnToPatrolAfterSeconds(3f));
                }
                break;
        }
    }


    // -------- AI BEHAVIORS --------

    void PatrolLogic()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPatrolIndex];

        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        MoveToward(target.position);

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", true);
        MoveToward(player.position);
    }

    void MoveToward(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position);
        float distance = direction.magnitude;

        if (distance > 5) // Move only if not already close enough
        {
            // direction.Normalize();
            // transform.position += direction * speed; // Fixed units per frame
            if (animator.GetBool("isRunning")) {
                transform.Translate(direction.normalized * runningSpeed * Time.deltaTime, Space.World);
            } else {
                transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            // transform.position = targetPos; // Snap to target if within 1 step
            transform.Translate(Vector3.zero, Space.World);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        // Smooth rotation toward target
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    void SearchForPlayer()
    {
        // TODO: stand still, look around - based on NavMesh
    }

    IEnumerator ReturnToPatrolAfterSeconds(float seconds)
    {
        isReturningToPatrol = true;
        yield return new WaitForSeconds(seconds);
        currentState = State.Patrol;
        isReturningToPatrol = false;
        
        Debug.Log("Returning to patrol.");
    }
}
