using UnityEngine;

public class AuditionRoomManager : MonoBehaviour
{
    public AuditionCenterManager manager;
    public AudioSource applause;

    private bool hasAuditioned = false;

    // particle system for this audition center
    public ParticleSystem starsParticleSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasAuditioned) {
                applause.Play();
                manager.PlayerAuditioned();
                hasAuditioned = true;
                Debug.Log("stopping particle system");
                starsParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                bool isParticlePlaying = starsParticleSystem.isPlaying;
                Debug.Log("Particle System Playing: " + isParticlePlaying);

            }
        }
    }
}
