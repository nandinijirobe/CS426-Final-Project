using UnityEngine;

public class DeductTime : MonoBehaviour
{
    public AudioClip errorSound; 
    private AudioSource audioSource; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (errorSound != null)
            {
                audioSource.clip = errorSound; 
                audioSource.Play();  
            }
        }
    }
}
