using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ManController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpHeight = 2f;
    public float jumpDuration = 1.625f;
    public float turnDuration = 0.3f;

    private Animator animator;
    private bool isJumping = false;
    private float jumpStartTime;
    private Vector3 jumpStartPos;
    private Vector3 jumpDirection;
    private float jumpSpeed;

    private bool isTurning = false;
    private float turnTimer = 0f;
    private Quaternion turnStartRotation;
    private Quaternion turnTargetRotation;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isTurning)
        {
            HandleTurn();
        }
        else if (isJumping)
        {
            HandleJumpMotion();
        }
        else
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(moveZ) > 0.01f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Smooth 180 turn on 'S' key press
        if (Input.GetKeyDown(KeyCode.S))
        {
            isTurning = true;
            turnTimer = 0f;
            turnStartRotation = transform.rotation;
            turnTargetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);
            return; // Don't process movement this frame
        }

        // Rotate with A and D
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -100f * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up, 100f * Time.deltaTime);

        Vector3 move = transform.forward * moveZ;
        if (isMoving)
        {
            transform.position += move.normalized * currentSpeed * Time.deltaTime;
        }

        // Animator parameters
        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", false);

        if (jumpPressed)
        {
            StartJump(move.normalized, currentSpeed);
        }
    }

    void HandleTurn()
    {
        turnTimer += Time.deltaTime;
        float t = Mathf.Clamp01(turnTimer / turnDuration);
        transform.rotation = Quaternion.Slerp(turnStartRotation, turnTargetRotation, t);

        if (t >= 1f)
        {
            isTurning = false;
        }
    }

    void StartJump(Vector3 direction, float speed)
    {
        isJumping = true;
        jumpStartTime = Time.time;
        jumpStartPos = transform.position;
        jumpDirection = direction;
        jumpSpeed = speed;

        animator.SetBool("isJumping", true);
    }

    void HandleJumpMotion()
    {
        float elapsed = Time.time - jumpStartTime;
        float progress = Mathf.Clamp01(elapsed / jumpDuration);

        float height = 4 * jumpHeight * progress * (1 - progress);
        Vector3 verticalOffset = new Vector3(0, height, 0);
        Vector3 forwardMove = jumpDirection * jumpSpeed * Time.deltaTime;

        transform.position += forwardMove;
        transform.position = new Vector3(transform.position.x, jumpStartPos.y + verticalOffset.y, transform.position.z);

        if (progress >= 1f)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }
}
