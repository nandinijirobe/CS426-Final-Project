using UnityEngine;
using UnityEngine.Scripting;

public class GlowToggle : MonoBehaviour
{
    public Material glowMaterial;
    public float glowOnIntensity = 2f;
    public float glowOffIntensity = 0f;
    public float duration = 10f;

    private Coroutine glowCoroutine;

    public void Awake()
    {
        glowMaterial.SetFloat("_GlowIntensity", glowOffIntensity);
    }

    public void TriggerGlow()
    {
        if (glowCoroutine != null)
            StopCoroutine(glowCoroutine);

        glowCoroutine = StartCoroutine(GlowForSeconds());
    }

    private System.Collections.IEnumerator GlowForSeconds()
    {
        glowMaterial.SetFloat("_GlowIntensity", glowOnIntensity);
        yield return new WaitForSeconds(duration);
        glowMaterial.SetFloat("_GlowIntensity", glowOffIntensity);
    }
}
