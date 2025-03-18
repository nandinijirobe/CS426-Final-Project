using UnityEngine;

public class CarMover : MonoBehaviour
{
    // Adjust this to set the forward speed (units per second)
    public float speed = 10f;
    // Adjust this to control how quickly the car turns (degrees per second)
    public float rotationSpeed = 30f;

    void Update()
    {
        // Move the car forward continuously
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // Rotate the car around the Y axis to create a turning effect
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
