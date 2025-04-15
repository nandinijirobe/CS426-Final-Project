using UnityEngine;

public class AuditionRoomManager : MonoBehaviour
{
    public AuditionCenterManager manager;
    public AudioSource applause;

    private bool hasAuditioned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasAuditioned) {
                applause.Play();
                manager.PlayerAuditioned();
                hasAuditioned = true;
            }
        }
    }
}
