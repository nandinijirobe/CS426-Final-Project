using UnityEngine;

public class PaparazzoSounds : MonoBehaviour
{
    public AudioClip cameraFlash;

    public void PlayFlashSound()
    {
        if (cameraFlash != null)
        {
            GetComponent<AudioSource>()?.PlayOneShot(cameraFlash);
        }
    }
}
