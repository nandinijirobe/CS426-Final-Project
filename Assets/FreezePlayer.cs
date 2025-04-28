using System.Collections;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{

    public FlockManager FM;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            Vector3 safespot = FM.safeSpot;
            player.transform.position = safespot;
            FreezePlayerMovement(true); // freeze the player
            StartCoroutine(UnfreezePlayerAfterDelay(10f));
        }
    }

    private void FreezePlayerMovement(bool freeze)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = freeze; 
        }
    }

    private IEnumerator UnfreezePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        FreezePlayerMovement(false);
    }
}
