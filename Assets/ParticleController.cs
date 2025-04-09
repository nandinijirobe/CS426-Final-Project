using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem papparaziParticleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playParticleSystem()
    {
        papparaziParticleSystem.Play();
    }

    public void stopParticleSystem()
    {
        papparaziParticleSystem.Stop();
    }
}
