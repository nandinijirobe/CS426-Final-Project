using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ManController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpHeight = 2f;
    public float jumpDuration = 1.625f;

    public GameObject cameraObject;
    public Vector3 cameraOffset = new Vector3(0, 5, -7);

    private Animator animator;
    private bool isJumping = false;
    private float jumpStartTime;
    private Vector3 jumpStartPos;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (cameraObject == null)
        {
            Debug.LogWarning("Camera Object not assigned!");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCamera();
    }

    void HandleMovement()
    {
        if (isJumping)
        {
            HandleJumpMotion();
            return;
        }

        float moveZ = Input.GetAxis("Vertical");   // W/S
        float moveX = Input.GetAxis("Horizontal"); // A/D
        bool isMoving = moveZ != 0 || moveX != 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        transform.position += move.normalized * speed * Time.deltaTime;

        // Rotate with Q and E
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -100f * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, 100f * Time.deltaTime);

        // Animator parameters
        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", false);

        // Jump
        if (jumpPressed && !isJumping)
        {
            StartJump();
        }
    }

    void StartJump()
    {
        isJumping = true;
        jumpStartTime = Time.time;
        jumpStartPos = transform.position;
        animator.SetBool("isJumping", true);
    }

    void HandleJumpMotion()
    {
        float elapsed = Time.time - jumpStartTime;
        float progress = Mathf.Clamp01(elapsed / jumpDuration);

        // Parabolic jump curve: smooth up and down
        float height = 4 * jumpHeight * progress * (1 - progress);
        Vector3 pos = jumpStartPos + new Vector3(0, height, 0);
        transform.position = new Vector3(transform.position.x, pos.y, transform.position.z);

        if (progress >= 1f)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    void HandleCamera()
    {
        if (cameraObject != null)
        {
            // cameraObject.transform.position = transform.position + transform.rotation * cameraOffset;
            // cameraObject.transform.LookAt(transform.position + Vector3.up * 1.5f);
        }
    }
}
