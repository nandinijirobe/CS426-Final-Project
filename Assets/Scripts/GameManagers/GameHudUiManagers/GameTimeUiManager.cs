using UnityEngine;
using TMPro; // If using TextMeshPro

public class GameTimeUiManager : MonoBehaviour
{
    [Header("References")]
    public SunriseSunset SunriseSunsetManager; // Link to your cycle script
    public TMP_Text tmpText; // ← Use this if using TextMeshPro

    [Header("Time Settings")]
    public int totalMinutesInDay = 1440; // 1440 = 24 hours x 60 mins

    public string prefix = "Clock: ";

    public enum TimeBlock { Morning, Noon, Evening, Night }
    public TimeBlock CurrentTimeBlock { get; private set; }

    void Update()
    {
        if (SunriseSunsetManager == null) return;

        // Convert timeOfDay (0-1) to total minutes
        float normalized = Mathf.Clamp01(SunriseSunsetManager.TimeOfDay);
        int currentMinutes = Mathf.FloorToInt(normalized * totalMinutesInDay);

        int hours = currentMinutes / 60;
        int minutes = currentMinutes % 60;

        // Update time block for external use
        if (hours >= 6 && hours < 12) CurrentTimeBlock = TimeBlock.Morning;
        else if (hours >= 12 && hours < 17) CurrentTimeBlock = TimeBlock.Noon;
        else if (hours >= 17 && hours < 21) CurrentTimeBlock = TimeBlock.Evening;
        else CurrentTimeBlock = TimeBlock.Night;

        string timeString = $"{prefix} {hours:D2}:{minutes:D2}";

        if (tmpText != null)
            tmpText.text = timeString;
    }
}
