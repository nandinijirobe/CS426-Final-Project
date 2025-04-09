using UnityEditor.Animations;
using UnityEngine;

public class PapTestController : MonoBehaviour
{

    Vector3 target;

    float speed = 2.5f;
    
    [SerializeField] public Animator animator;

    bool isWalking = false;
    bool isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        SetNewTarget(new Vector3(transform.position.x - 100, transform.position.y, transform.position.z - 100));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        
        isWalking = true;
        animator.SetBool("isWalking", isWalking && !isRunning);
    }

    void SetNewTarget(Vector3 newTarget) 
    {
        target = newTarget;
        transform.LookAt(target);
    }
}
