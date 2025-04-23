using System.Threading;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody[] ragdollBodies;
    public Transform ragdollRootBone;

    public float movementThreshold = 0.05f; // how still is "still"
    public float stillTimeRequired = 2f;    // how long to be still before reset

    private bool isRagdoll = false;
    private float RDTimer = 0f;

    void Start()
    {
        // ragdollBodies = GetComponentsInChildren<Rigidbody>();
        SetRagdollState(false); // Start with ragdoll disabled
    }

    void Update()
    {
        if (isRagdoll)
        {
            if (RDTimer > 5) {
                ResetToAnimation();
                isRagdoll = false;
            }
            else {
                RDTimer += 1;
            }
        }
    }

    public void SetRagdollState(bool input)
    {
        isRagdoll = input;
        animator.enabled = !isRagdoll;
        
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = !isRagdoll;
            rb.GetComponent<Collider>().enabled = isRagdoll;
        }
    }

    private void ResetToAnimation()
    {
        // Snap root to ragdoll's current hip bone position
        if (ragdollRootBone != null)
        {
            Vector3 ragdollPos = ragdollRootBone.position;
            transform.position = ragdollPos;
            transform.rotation = Quaternion.Euler(0, ragdollRootBone.rotation.eulerAngles.y, 0); // preserve forward
        }

        SetRagdollState(false);
        RDTimer = 0;
        Debug.Log("Resetting from ragdoll!");
    }
}
