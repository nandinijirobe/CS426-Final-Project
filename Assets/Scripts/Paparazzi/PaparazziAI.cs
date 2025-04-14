using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PaparazziAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public BayesianDecisionManager bayesManager;
    public ParticleController PaparazziParticleController;

    public float chaseThreshold = 0.6f;
    public float viewDistance = 10f;
    public float rotationSpeed = 5f;

    [Header("Reference to Player's Money Manager")]
    public PlayerMoneyManager playerMoneyManager;

    [Header("Animation")]
    public Animator animator;

    [Header("Camera Flash Light")]
    public Light flashLight;
    public float flashDuration = 0.1f;

    [Header("Movement Speeds")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    [Header("Chase Cooldown After Catching Player")]
    public float postCatchCooldown = 10f;
    private float chaseCooldownTimer = 0f;

    private int currentPatrolIndex = 0;
    private enum State { Patrol, Chase, Search }
    private State currentState = State.Patrol;

    private bool isReturningToPatrol = false;
    private Rigidbody rb;

    private Vector3? currentTarget = null;
    private float currentSpeed = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        flashLight.enabled = false;
        if (patrolPoints.Length > 0)
            rb.MovePosition(patrolPoints[0].position);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool shouldChase = bayesManager.ShouldChase();

        if (chaseCooldownTimer > 0)
            chaseCooldownTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Patrol:
                PatrolLogic();
                PaparazziParticleController.stopParticleSystem();
                SetAnimationState(true, false);

                if (chaseCooldownTimer <= 0 && shouldChase && distanceToPlayer < viewDistance)
                {
                    currentState = State.Chase;
                    Debug.Log("Switching to Chase!");
                }
                break;

            case State.Chase:
                ChasePlayer();
                PaparazziParticleController.playParticleSystem();
                SetAnimationState(false, true);

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

    private void FixedUpdate()
    {
        if (currentTarget.HasValue)
        {
            Vector3 direction = (currentTarget.Value - rb.position).normalized;
            Vector3 newPos = rb.position + direction * currentSpeed * Time.fixedDeltaTime;

            float distance = Vector3.Distance(rb.position, currentTarget.Value);
            if (distance > 0.1f)
            {
                rb.MovePosition(newPos);
                RotateToward(direction);
            }
            else
            {
                currentTarget = null; // reached
            }
        }
    }

    void PatrolLogic()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPatrolIndex];
        SetMoveTarget(target.position, walkSpeed);

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        SetMoveTarget(player.position, runSpeed);
    }

    void SetMoveTarget(Vector3 targetPos, float speed)
    {
        currentTarget = targetPos;
        currentSpeed = speed;
    }

    void RotateToward(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerMoneyManager != null)
        {
            playerMoneyManager.DeductMoney();
            StartCoroutine(FlashLight());

            chaseCooldownTimer = postCatchCooldown;
            currentState = State.Patrol;
            Debug.Log("Caught player. Returning to patrol with cooldown.");
        }
    }

    void SetAnimationState(bool isWalking, bool isRunning)
    {
        if (animator == null) return;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);
    }

    IEnumerator ReturnToPatrolAfterSeconds(float seconds)
    {
        isReturningToPatrol = true;
        yield return new WaitForSeconds(seconds);
        currentState = State.Patrol;
        isReturningToPatrol = false;
        Debug.Log("Returning to patrol.");
    }

    IEnumerator FlashLight()
    {
        if (flashLight != null)
        {
            flashLight.enabled = true;
            yield return new WaitForSeconds(flashDuration);
            flashLight.enabled = false;
        }
    }
}
