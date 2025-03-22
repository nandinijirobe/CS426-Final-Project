using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ManController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    public GameObject cameraObject; // Assign your Main Camera here in the Inspector
    public Vector3 cameraOffset = new Vector3(0, 5, -7); // Adjust this as needed

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (cameraObject == null)
        {
            Debug.LogWarning("Camera Object not assigned!");
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        UpdateCamera();
    }

    void MovePlayer()
    {
        float moveZ = Input.GetAxis("Vertical");   // W/S
        float moveX = Input.GetAxis("Horizontal"); // A/D

        // Movement in local forward direction
        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        Vector3 newVelocity = move.normalized * moveSpeed;
        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z); // keep y velocity for gravity

        // Rotation with Q (left) and E (right)
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime);
        }

        // Animation
        bool isMoving = moveZ != 0 || moveX != 0;
        animator.SetBool("isRunning", isMoving);
    }

    void UpdateCamera()
    {
        if (cameraObject != null)
        {
            cameraObject.transform.position = transform.position + cameraOffset;
            cameraObject.transform.LookAt(transform.position + Vector3.up * 1.5f); // adjust for head height
        }
    }
}
