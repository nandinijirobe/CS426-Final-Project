using TMPro;
using UnityEngine;

public class AuditionCenterManager : MonoBehaviour
{

    // inset game manager here
    public AudioSource music;
    //public AudioSource bgMusic;
    public GameObject stageDoor;
    public ParticleSystem particles;

    public GameGeneralManager gameGeneralManager;
    bool hasAuditioned = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasAuditioned) {
                music.Play();
                //bgMusic.Stop();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && hasAuditioned)
        {
            stageDoor.SetActive(true); // close the door so player can't reenter
            //bgMusic.Play();
            particles.Stop();

        }
    }
    public void PlayerAuditioned() 
    {
        if (!hasAuditioned) {
            hasAuditioned = true;
            gameGeneralManager.updateAuditionCount();
        }
    }
}
