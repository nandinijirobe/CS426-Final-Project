using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class ProtagonistController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpHeight = 2f;
    public float jumpDuration = 1.625f;
    public float turnDuration = 0.3f;

    [Header("Audio Clips")]
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip jumpClip;

    private bool isMovingPrev = false;
    private bool isRunningPrev = false;
    private bool wasJumping = false;


    private Animator animator;
    private Rigidbody rb;

    private bool isJumping = false;
    private float jumpStartTime;
    private Vector3 jumpStartPos;
    private Vector3 jumpDirection;
    private float jumpSpeed;

    private bool isTurning = false;
    private float turnTimer = 0f;
    private Quaternion turnStartRotation;
    private Quaternion turnTargetRotation;

    private Vector3 moveIntent = Vector3.zero;
    private float moveSpeed = 0f;
    private bool jumpTriggered = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isTurning)
        {
            HandleTurn();
        }
        else if (isJumping)
        {
            // Do jump motion in Update to keep Time.time-based curve smooth
            HandleJumpMotion();
        }
        else
        {
            HandleMovementInput();
        }
    }

    void FixedUpdate()
    {
        if (!isJumping && !isTurning && moveIntent != Vector3.zero)
        {
            Vector3 newPos = rb.position + moveIntent * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    void HandleMovementInput()
    {
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(moveZ) > 0.01f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        moveSpeed = isRunning ? runSpeed : walkSpeed;
        moveIntent = isMoving ? transform.forward * moveZ : Vector3.zero;

        // Smooth 180 turn on 'S' key press
        if (Input.GetKeyDown(KeyCode.S))
        {
            isTurning = true;
            turnTimer = 0f;
            turnStartRotation = transform.rotation;
            turnTargetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);
            moveIntent = Vector3.zero;
            return;
        }

        // Rotate with A and D
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -100f * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up, 100f * Time.deltaTime);

        // Animator states
        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", false);

        // Start jump
        if (jumpPressed)
        {
            StartJump(moveIntent.normalized, moveSpeed);
            PlayOneShot(jumpClip);
        }

        // Movement sound logic
        if (isMoving && !isRunning)
        {
            if (!audioSource.isPlaying || audioSource.clip != walkClip)
                PlayLoop(walkClip);
        }
        else if (isMoving && isRunning)
        {
            if (!audioSource.isPlaying || audioSource.clip != runClip)
                PlayLoop(runClip);
        }
        else
        {
            StopLoop();
        }
    }

    void PlayLoop(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void StopLoop()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void PlayOneShot(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
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

        // Parabolic height
        float height = 4 * jumpHeight * progress * (1 - progress);
        Vector3 verticalOffset = new Vector3(0, height, 0);

        // ✅ Accumulate forward movement over time (use elapsed)
        Vector3 forwardMove = jumpDirection * jumpSpeed * elapsed;
        Vector3 jumpTarget = jumpStartPos + forwardMove + verticalOffset;

        rb.MovePosition(jumpTarget);

        if (progress >= 1f)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }
}
