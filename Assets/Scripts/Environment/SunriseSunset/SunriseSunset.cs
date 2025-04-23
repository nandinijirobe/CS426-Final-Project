using UnityEngine;

public class SunriseSunset : MonoBehaviour
{
    [Header("Sun Settings")]
    public Light sun;
    public float dayDuration = 120f; // Full day cycle duration in seconds
    public Gradient sunColor; // Color throughout the day
    public AnimationCurve sunIntensity; // Intensity curve through the day

    [Header("Skybox Settings (Optional)")]
    public bool adjustSkybox = true;
    public Material skyboxMaterial; // Assign procedural skybox material
    public AnimationCurve atmosphereThicknessCurve;
    public AnimationCurve exposureCurve;

    [Header("Fog & Ambient Settings (Optional)")]
    public bool adjustFogAndAmbient = true;
    public Color dayAmbientColor = Color.white;
    public Color nightAmbientColor = Color.black;
    public Color dayFogColor = new Color(0.7f, 0.8f, 1f, 1f);
    public Color nightFogColor = new Color(0.05f, 0.05f, 0.1f, 1f);

    [Header("Moonlight (Optional)")]
    public bool useMoon = true;
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Range(0f, 24f)]
    public float startHour = 1f; // Starts at 1AM by default


    [SerializeField] private float timeOfDay;
    public float TimeOfDay => timeOfDay; // public read-only access // 0 - 1 representing progress through the day

    void Start()
    {
        if (sun == null) Debug.LogWarning("Sun is not assigned!");
        if (skyboxMaterial != null) RenderSettings.skybox = skyboxMaterial;
        if (useMoon && moon == null) Debug.LogWarning("Moon is enabled but not assigned!");
    }

    void Update()
    {
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay > 1f) timeOfDay = 0f;

        UpdateSun();
        if (adjustSkybox) UpdateSkybox();
        if (adjustFogAndAmbient) UpdateEnvironment();
        if (useMoon) UpdateMoon();
    }

    void UpdateSun()
    {
        float sunAngle = (timeOfDay * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0);
        sun.color = sunColor.Evaluate(timeOfDay);
        sun.intensity = sunIntensity.Evaluate(timeOfDay);
    }

    void UpdateMoon()
    {
        float moonAngle = ((timeOfDay + 0.5f) * 360f) - 90f; // Offset by 12 hours
        moon.transform.rotation = Quaternion.Euler(moonAngle, 170f, 0);
        moon.color = moonColor.Evaluate(timeOfDay);
        moon.intensity = moonIntensity.Evaluate(timeOfDay);
    }

    void UpdateSkybox()
    {
        if (skyboxMaterial == null) return;
        float thickness = atmosphereThicknessCurve.Evaluate(timeOfDay);
        float exposure = exposureCurve.Evaluate(timeOfDay);
        skyboxMaterial.SetFloat("_AtmosphereThickness", thickness);
        skyboxMaterial.SetFloat("_Exposure", exposure);
    }

    void UpdateEnvironment()
    {
        RenderSettings.ambientLight = Color.Lerp(nightAmbientColor, dayAmbientColor, GetDaylightFactor());
        RenderSettings.fogColor = Color.Lerp(nightFogColor, dayFogColor, GetDaylightFactor());
    }

    float GetDaylightFactor()
    {
        // 0 at midnight, 1 at noon
        return Mathf.Clamp01(Mathf.Cos((timeOfDay - 0.5f) * 2 * Mathf.PI));
    }
}
