using UnityEngine;
using TMPro;

public class GameTimeUiManager : MonoBehaviour
{
    [Header("References")]
    public SunriseSunset SunriseSunsetManager;
    public TMP_Text tmpText;

    [Header("Time Settings")]
    public int totalMinutesInDay = 1440; // 1440 = 24h
    public string prefix = "Clock: ";

    [Header("End Game Warning Settings")]
    public int warningMinutesLeft = 10;
    public AudioClip warningSound;
    public AudioSource audioSource;

    [Header("Activation")]
    public bool isActive = false;  //  Toggle this to start game time

    private bool warningPlayed = false;

    public enum TimeBlock { Morning, Noon, Evening, Night }
    public TimeBlock CurrentTimeBlock { get; private set; }

    void Update()
    {
        if (!isActive || SunriseSunsetManager == null) return;

        float normalized = Mathf.Clamp01(SunriseSunsetManager.TimeOfDay);
        int currentMinutes = Mathf.FloorToInt(normalized * totalMinutesInDay);

        int hours = currentMinutes / 60;
        int minutes = currentMinutes % 60;

        // Time block logic
        if (hours >= 6 && hours < 12) CurrentTimeBlock = TimeBlock.Morning;
        else if (hours >= 12 && hours < 17) CurrentTimeBlock = TimeBlock.Noon;
        else if (hours >= 17 && hours < 21) CurrentTimeBlock = TimeBlock.Evening;
        else CurrentTimeBlock = TimeBlock.Night;

        // Time display
        string timeString = $"{prefix} {hours:D2}:{minutes:D2}";
        if (tmpText != null)
            tmpText.text = timeString;

        // Warning sound logic
        int minutesRemaining = totalMinutesInDay - currentMinutes;
        if (!warningPlayed && minutesRemaining <= warningMinutesLeft)
        {
            if (audioSource != null && warningSound != null)
            {
                audioSource.PlayOneShot(warningSound);
                warningPlayed = true;
            }
        }
    }
}
