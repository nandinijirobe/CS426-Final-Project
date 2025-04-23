using UnityEngine;
using UnityEngine.Android;

public class CarControlller : MonoBehaviour
{
    Rigidbody rb;


    private float power = 300; // speed of car
    private float torque = 300f; // agility of car?
    
    private float maxSpeed = 200;

    private Vector2 movementVector;

    private float brakingPower = 70f;
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

        if (movementVector == Vector2.zero)
        {
            // Apply a strong drag to stop the car quickly
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.zero, brakingPower * Time.fixedDeltaTime);
            rb.angularVelocity = Vector3.MoveTowards(rb.angularVelocity, Vector3.zero, brakingPower * Time.fixedDeltaTime);
        }
        else
        {
            // Accelerate forward
            if (rb.linearVelocity.magnitude < maxSpeed)
            {
                rb.AddForce(movementVector.y * transform.forward * power);
            }

            // Turning
            rb.AddTorque(movementVector.x * Vector3.up * torque * movementVector.y);
        }

        ApplyLateralFriction();
    }

    void ApplyLateralFriction()
    {
        Vector3 lateralVelocity = Vector3.Dot(rb.linearVelocity, transform.right) * transform.right;
        rb.AddForce(-lateralVelocity * 5f, ForceMode.Acceleration); // adjust 5f as needed
    }
}
