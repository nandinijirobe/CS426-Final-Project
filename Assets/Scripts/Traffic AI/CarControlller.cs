using UnityEngine;
using UnityEngine.Android;

public class CarControlller : MonoBehaviour
{
    Rigidbody rb;


    [SerializeField]
    private float power = 5; // speed of car
    [SerializeField]
    private float torque = 0.5f; // agility of car?
    [SerializeField]
    private float maxSpeed = 5;

    [SerializeField]
    private Vector2 movementVector;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 movementInput)
    {
        this.movementVector = movementInput;
    }

    private void FixedUpdate() 
    {
        if(rb.linearVelocity.magnitude < maxSpeed) {
            rb.AddForce(movementVector.y * transform.forward*power);
        }
        rb.AddTorque(movementVector.x*Vector3.up*torque*movementVector.y); // for turning
    }
}
