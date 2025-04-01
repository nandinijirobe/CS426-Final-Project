using UnityEngine;

public class paparazzoSounds : MonoBehaviour
{
    public AudioClip cameraFlash;

    void OnTriggerEnter(Collider other) // when door is hit by an object with tag player, play sound and start rotating.
    {
        Debug.Log("papparazzo hit something: " + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("papparazzo hit player");
            GetComponent<AudioSource>().PlayOneShot(cameraFlash);
        }
    }
}
