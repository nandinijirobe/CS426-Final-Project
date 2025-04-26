using UnityEngine;

public class PlayerSoundFXHandler : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip gruntSFX;
    public AudioClip walkSFX;
    public AudioClip runSFX;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void PlayerRollSound()
    {
        audioSource.PlayOneShot(gruntSFX);
    }
}
