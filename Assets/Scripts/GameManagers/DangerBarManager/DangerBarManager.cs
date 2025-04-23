using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DangerBarManager : MonoBehaviour
{
    [Header("Danger Settings")]
    public Slider dangerBar;
    public float fillSpeed = 0.25f;
    public float decaySpeed = 0.2f;
    public float dangerThreshold = 1f;

    [Header("UI")]
    public CanvasGroup dangerBarCanvasGroup;
    public Image fillImage;
    public Color safeColor = Color.green;
    public Color midColor = Color.yellow;
    public Color dangerColor = Color.red;
    public float fadeOutDuration = 1f;

    [Header("Shader")]
    public Material dangerBarMaterial; // Material using your DangerBarGlow.shader

    [Header("Penalty Flash UI")]
    public CanvasGroup penaltyCanvasGroup;  // CanvasGroup on the flash image
    public float penaltyDisplayTime = 2f;

    private float dangerLevel = 0f;
    private PaparazziAI activeThreat;

    private List<PaparazziAI> activePaparazzi = new();
    private bool isInCooldown = false;
    private bool isBarVisible = false;
    private bool dangerTriggered = false;
    private float fadeOutTimer = 0f;

    void Awake()
    {
        if (dangerBarCanvasGroup != null)
            dangerBarCanvasGroup.alpha = 0f;

        if (penaltyCanvasGroup != null)
        {
            penaltyCanvasGroup.alpha = 0f;
            penaltyCanvasGroup.gameObject.SetActive(false);
        }

        isBarVisible = false;
        dangerLevel = 0f;

        if (dangerBarMaterial == null && fillImage != null)
            dangerBarMaterial = fillImage.material; // Try to auto-assign
    }

    void Update()
    {
        if (isInCooldown) return;

        float cumulativeDangerRate = 0f;

        foreach (var paparazzi in activePaparazzi)
        {
            float proximity = paparazzi.GetProximityLevel();
            cumulativeDangerRate += proximity * fillSpeed * Time.deltaTime;
        }

        if (cumulativeDangerRate > 0)
        {
            dangerLevel += cumulativeDangerRate;
        }
        else
        {
            dangerLevel -= decaySpeed * Time.deltaTime;
        }

        dangerLevel = Mathf.Clamp01(dangerLevel);
        dangerBar.value = dangerLevel;

        UpdateBarColor(dangerLevel);
        UpdateShaderFill(dangerLevel);

        if (dangerLevel > 0.01f && !isBarVisible)
        {
            dangerBarCanvasGroup.alpha = 1f;
            isBarVisible = true;
        }

        if (dangerLevel <= 0 && activePaparazzi.Count == 0 && isBarVisible)
        {
            fadeOutTimer += Time.deltaTime;
            dangerBarCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeOutTimer / fadeOutDuration);

            if (fadeOutTimer >= fadeOutDuration)
            {
                isBarVisible = false;
                fadeOutTimer = 0f;
            }
        }

        if (dangerLevel >= dangerThreshold && !dangerTriggered && activeThreat != null)
        {
            dangerTriggered = true;
            dangerLevel = 0f;
            dangerBar.value = 0f;
            UpdateShaderFill(0f);

            activeThreat.TriggerFlashAndPenalty();
            ShowPenaltyImage();
            StartCooldown();
        }
    }

    void StartCooldown()
    {
        isInCooldown = true;
        dangerBarCanvasGroup.alpha = 0f;
        isBarVisible = false;
        fadeOutTimer = 0f;
        dangerTriggered = false;

        Invoke(nameof(EndCooldown), activeThreat.postCatchCooldown);
    }

    void EndCooldown()
    {
        isInCooldown = false;
    }

    public void OnPaparazziEnter(PaparazziAI paparazzi)
    {
        if (!activePaparazzi.Contains(paparazzi))
            activePaparazzi.Add(paparazzi);

        activeThreat = paparazzi;
        dangerBarCanvasGroup.alpha = 1f;
        isBarVisible = true;
    }

    public void OnPaparazziExit(PaparazziAI paparazzi)
    {
        activePaparazzi.Remove(paparazzi);

        if (activePaparazzi.Count == 0)
        {
            activeThreat = null;
            fadeOutTimer = 0f;
        }
    }

    void UpdateBarColor(float level)
    {
        if (fillImage == null) return;

        if (level < 0.3f)
            fillImage.color = safeColor;
        else if (level < 0.7f)
            fillImage.color = midColor;
        else
            fillImage.color = dangerColor;
    }

    void UpdateShaderFill(float level)
    {
        if (dangerBarMaterial != null)
        {
            dangerBarMaterial.SetFloat("_FillAmount", level);
        }
    }

    void ShowPenaltyImage()
    {
        if (penaltyCanvasGroup != null)
        {
            penaltyCanvasGroup.alpha = 0.8f;
            penaltyCanvasGroup.gameObject.SetActive(true);
            StartCoroutine(FadeOutPenaltyImage());
        }
    }

    IEnumerator FadeOutPenaltyImage()
    {
        float timer = 0f;
        float startAlpha = penaltyCanvasGroup.alpha;

        while (timer < penaltyDisplayTime)
        {
            timer += Time.deltaTime;
            float t = timer / penaltyDisplayTime;
            penaltyCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        penaltyCanvasGroup.alpha = 0f;
        penaltyCanvasGroup.gameObject.SetActive(false);
    }
}
