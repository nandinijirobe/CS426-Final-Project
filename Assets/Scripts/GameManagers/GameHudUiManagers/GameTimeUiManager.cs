using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimeUiManager : MonoBehaviour
{
    [Header("References")]
    public SunriseSunset SunriseSunsetManager;
    public TMP_Text tmpText;

    [Header("Time Settings")]
    public int totalMinutesInDay = 1440; // 1440 = 24h countdown
    public string prefix = "Time Left: ";

    [Header("End Game Warning Settings")]
    public int yellowThresholdMinutes = 60;
    public int redThresholdMinutes = 10;
    public AudioClip warningSound;
    public AudioSource audioSource;

    [Header("Activation")]
    public bool isActive = false;

    [Header("Game End Settings")]
    public GameObject gameLoseEndScreenUI;
    public bool gameHasEnded = false;

    [Header("Game Restart")]
    public float restartDelay = 5f;

    private bool warningPlayed = false;
    private Vector3 originalScale;
    private Color greenColor = Color.green;
    private Color yellowColor = Color.yellow;
    private Color redColor = Color.red;

    public enum TimeBlock { Morning, Noon, Evening, Night }
    public TimeBlock CurrentTimeBlock { get; private set; }

    void Start()
    {
        if (gameLoseEndScreenUI != null)
            gameLoseEndScreenUI.SetActive(false);

        warningPlayed = false;
        gameHasEnded = false;

        if (tmpText != null)
            originalScale = tmpText.transform.localScale;
    }

    void Update()
    {
        if (!isActive || SunriseSunsetManager == null) return;

        float normalized = Mathf.Clamp01(SunriseSunsetManager.TimeOfDay);
        int elapsedMinutes = Mathf.FloorToInt(normalized * totalMinutesInDay);

        int minutesLeft = Mathf.Max(totalMinutesInDay - elapsedMinutes, 0);

        int hours = minutesLeft / 60;
        int minutes = minutesLeft % 60;

        // Display countdown
        string timeString = $"{prefix} {hours:D2}:{minutes:D2}";
        if (tmpText != null)
            tmpText.text = timeString;

        // Color change based on time remaining
        if (minutesLeft <= redThresholdMinutes)
        {
            tmpText.color = redColor;
            PulseText();
        }
        else if (minutesLeft <= yellowThresholdMinutes)
        {
            tmpText.color = yellowColor;
            ResetPulse();
        }
        else
        {
            tmpText.color = greenColor;
            ResetPulse();
        }

        // Warning sound
        if (!warningPlayed && minutesLeft <= redThresholdMinutes)
        {
            if (audioSource != null && warningSound != null)
            {
                audioSource.PlayOneShot(warningSound);
                warningPlayed = true;
            }
        }

        // End of game
        if (!gameHasEnded && minutesLeft <= 0)
        {
            gameHasEnded = true;
            TriggerGameEnd();
        }
    }

    void PulseText()
    {
        if (tmpText != null)
        {
            float scale = 1f + 0.1f * Mathf.Sin(Time.time * 5f); // 5Hz wave
            tmpText.transform.localScale = originalScale * scale;
        }
    }

    void ResetPulse()
    {
        if (tmpText != null)
            tmpText.transform.localScale = originalScale;
    }

    void TriggerGameEnd()
    {
        Debug.Log("Time is up!");

        if (gameLoseEndScreenUI != null)
            gameLoseEndScreenUI.SetActive(true);

        StartCoroutine(RestartGameAfterDelay());
    }

    System.Collections.IEnumerator RestartGameAfterDelay()
    {
        yield return new WaitForSecondsRealtime(restartDelay);

        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
