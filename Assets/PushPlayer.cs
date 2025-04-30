using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Debug.Log("Colliding with the player");
                player.ApplyPush(transform.position);   
            }
        }
    }
}
