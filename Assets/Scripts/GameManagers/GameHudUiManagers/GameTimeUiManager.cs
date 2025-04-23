using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("Game End Settings")]
    public GameObject gameLoseEndScreenUI; // assign your UI panel in the Inspector
    public bool gameHasEnded = false;

    [Header("Game Restart")]
    public float restartDelay = 5f; // Seconds before game restarts

    private bool warningPlayed = false;

    public enum TimeBlock { Morning, Noon, Evening, Night }
    public TimeBlock CurrentTimeBlock { get; private set; }

    void Start()
    {
        if (gameLoseEndScreenUI != null)
            gameLoseEndScreenUI.SetActive(false);

        warningPlayed = false;
        gameHasEnded = false;
    }

    void Update()
    {
        if (!isActive || SunriseSunsetManager == null) return;

        float normalized = Mathf.Clamp01(SunriseSunsetManager.TimeOfDay);
        int currentMinutes = Mathf.FloorToInt(normalized * totalMinutesInDay);

        // Adjust with optional startHour offset
        float startHour = SunriseSunsetManager.startHour;  // Add a public getter if needed
        int adjustedMinutes = currentMinutes + Mathf.FloorToInt(startHour * 60) % totalMinutesInDay;

        int hours = (adjustedMinutes / 60) % 24;
        int minutes = adjustedMinutes % 60;

        // Time block logic
        if (hours >= 6 && hours < 12) CurrentTimeBlock = TimeBlock.Morning;
        else if (hours >= 12 && hours < 17) CurrentTimeBlock = TimeBlock.Noon;
        else if (hours >= 17 && hours < 21) CurrentTimeBlock = TimeBlock.Evening;
        else CurrentTimeBlock = TimeBlock.Night;

        // Time display
        string timeString = $"{prefix} {hours % 12:D2}:{minutes:D2} {(hours < 12 ? "AM" : "PM")}";
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

        // End of game logic — trigger at 11:59 PM
        if (!gameHasEnded && adjustedMinutes >= totalMinutesInDay - 1) // Last minute of the day
        {
            gameHasEnded = true;
            TriggerGameEnd();
        }
    }

    void TriggerGameEnd()
    {
        Debug.Log("Game day has ended at 11:59 PM!");

        if (gameLoseEndScreenUI != null)
            gameLoseEndScreenUI.SetActive(true);

        StartCoroutine(RestartGameAfterDelay());
    }

    System.Collections.IEnumerator RestartGameAfterDelay()
    {
        yield return new WaitForSecondsRealtime(restartDelay);

        Time.timeScale = 1f; // Reset in case it was paused
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
