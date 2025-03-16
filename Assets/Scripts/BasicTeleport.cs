using UnityEngine;

public class BasicTeleport : MonoBehaviour
{
    public Transform target = null;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit taxi");
        if (other.gameObject.tag == "Player") {
            Debug.Log("Entering Portal #1");
            other.gameObject.transform.position = target.position;
            Debug.Log("Going to Portal #2");
        }
    }
}