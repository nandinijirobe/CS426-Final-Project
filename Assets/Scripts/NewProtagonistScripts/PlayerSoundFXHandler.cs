using UnityEngine;

public class PlayerSoundFXHandler : MonoBehaviour
{
    public AudioSource audioSourceMovement;
    public AudioSource audioSourceActions;

    public AudioClip gruntSFX;
    public AudioClip walkSFX;
    public AudioClip sprintSFX;

    private void Awake()
    {
        // audioSource = GetComponent<AudioSource>();

    }

    public void PlayerRollSound()
    {
        audioSourceActions.PlayOneShot(gruntSFX);
    }
    public void PlayLoop(AudioClip clip)
    {
        if (audioSourceMovement != null && (clip != audioSourceMovement.clip || !audioSourceMovement.isPlaying))
        {
            audioSourceMovement.clip = clip;
            audioSourceMovement.loop = true; 
            audioSourceMovement.Play();
        }
    }

    public void StopLoop()
    {
        if (audioSourceMovement.isPlaying)
        {
            audioSourceMovement.Stop();
        }
    }
}
