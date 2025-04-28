using UnityEngine;

public class DeductTime : MonoBehaviour
{
    public AudioClip errorSound; 
    private AudioSource audioSource; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (errorSound != null)
            {
                audioSource.clip = errorSound; 
                audioSource.Play();  
            }
        }
    }
}
