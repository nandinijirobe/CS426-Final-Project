using UnityEngine;
using System;
using System.Collections.Generic;

public class BayesianDecisionManager : MonoBehaviour
{
    public enum TimeOfDay { Morning, Noon, Evening, Night }
    public enum StreetType { Alley, Regular, Famous }

    [Header("Bayesian Conditions")]
    public TimeOfDay currentTimeOfDay = TimeOfDay.Morning;
    public StreetType currentStreetType = StreetType.Regular;
    [Range(0f, 1f)] public float fameLevel = 0.1f; // 0–1 normalized

    public GameTimeUiManager timeUIManager; // assign in Inspector or via script

    [Header("Popularity Tracking")]
    [Range(0f, 1f)] public float popularityLevel = 0f; // 0 = unknown, 1 = ultra popular
    public UnityEngine.UI.Slider popularityBar; // UI bar in Inspector

    [Header("UI Styling")]
    public Gradient popularityColorGradient;
    public UnityEngine.UI.Image fillImage; // Assign the fill part of the slider

    private Dictionary<(TimeOfDay, StreetType, string), float> CPT;

    private void Awake()
    {
        CPT = new Dictionary<(TimeOfDay, StreetType, string), float>();

        string[] fameBuckets = { "low", "mid", "high" };
        TimeOfDay[] times = { TimeOfDay.Morning, TimeOfDay.Noon, TimeOfDay.Evening, TimeOfDay.Night };
        StreetType[] streets = { StreetType.Alley, StreetType.Regular, StreetType.Famous };

        foreach (var time in times)
        {
            foreach (var street in streets)
            {
                foreach (var fame in fameBuckets)
                {
                    float prob = 0.1f; // default

                    if (fame == "high") prob += 0.4f;
                    else if (fame == "mid") prob += 0.2f;

                    if (street == StreetType.Famous) prob += 0.3f;
                    else if (street == StreetType.Regular) prob += 0.1f;

                    if (time == TimeOfDay.Evening) prob += 0.2f;
                    else if (time == TimeOfDay.Night) prob -= 0.1f;

                    // Clamp
                    prob = Mathf.Clamp01(prob);

                    CPT[(time, street, fame)] = prob;
                }
            }
        }
    }

    void Start()
    {
        // if (popularityColorGradient.colorKeys.Length == 0) // if not set
        // {
            popularityColorGradient = new Gradient();
            popularityColorGradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.green, 0f),
                    new GradientColorKey(Color.yellow, 0.5f),
                    new GradientColorKey(Color.red, 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );
        // }
    }


    void Update()
    {
        if (timeUIManager != null)
        {
            // Convert TimeBlock to your local enum
            switch (timeUIManager.CurrentTimeBlock)
            {
                case GameTimeUiManager.TimeBlock.Morning:
                    currentTimeOfDay = TimeOfDay.Morning;
                    break;
                case GameTimeUiManager.TimeBlock.Noon:
                    currentTimeOfDay = TimeOfDay.Noon;
                    break;
                case GameTimeUiManager.TimeBlock.Evening:
                    currentTimeOfDay = TimeOfDay.Evening;
                    break;
                case GameTimeUiManager.TimeBlock.Night:
                    currentTimeOfDay = TimeOfDay.Night;
                    break;
            }
        }

        // 🔥 Update popularity level every frame based on the current chase probability
        popularityLevel = Mathf.Lerp(popularityLevel, GetChasingProbability(), Time.deltaTime * 5f);

        // Update UI bar
        if (popularityBar != null)
        {
            popularityBar.value = popularityLevel;
        }

        // Update the fill color based on the popularity level
        if (fillImage != null)
        {
            Color newColor = popularityColorGradient.Evaluate(popularityLevel);
            fillImage.color = newColor;
        }

        if (popularityLevel > 0.8f)
        {
            float pulse = Mathf.PingPong(Time.time * 2f, 0.2f);
            fillImage.color = popularityColorGradient.Evaluate(popularityLevel) * (1f + pulse);
        }

    }

    public string FameBucket()
    {
        if (fameLevel < 0.3f) return "low";
        else if (fameLevel < 0.7f) return "mid";
        else return "high";
    }

    public float GetChasingProbability()
    {
        var key = (currentTimeOfDay, currentStreetType, FameBucket());
        if (CPT.TryGetValue(key, out float prob))
        {
            return prob;
        }
        else
        {
            return 0.3f; // default fallback probability
        }
    }

    public bool ShouldChase()
    {
        float p = GetChasingProbability();
        float rand =  UnityEngine.Random.value;
        Debug.Log("rand: " + rand + ", p: " + p);
        return rand < p; // Randomize behavior
    }
}
