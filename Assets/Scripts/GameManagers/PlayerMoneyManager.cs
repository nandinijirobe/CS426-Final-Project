using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMoneyManager : MonoBehaviour
{
    [Header("Money Settings")]
    public int startingMoney = 500;

    [Header("UI")]
    public TMP_Text moneyText; // Assign this in the Inspector

    [Header("Game Over UI")]
    public GameObject gameOverScreen; // Assign this in the Inspector

    [Header("Game Restart")]
    public float restartDelay = 5f;

    private int currentMoney;

    void Start()
    {
        currentMoney = startingMoney;
        gameOverScreen.SetActive(false);
        UpdateMoneyUI();
    }

    // Call this when the player hits a paparazzi
    public void DeductMoney(int amount)
    {
        currentMoney -= amount;
        currentMoney = Mathf.Max(currentMoney, 0);
        UpdateMoneyUI();

        if (currentMoney <= 0)
        {
            Debug.Log("You're broke! Showing Game Over screen.");
            TriggerGameEnd();
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "$" + currentMoney.ToString("F0");
    }

    public float GetCurrentMoney()
    {
        return currentMoney;
    }

    public void AddMoney(int amount) 
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    private void TriggerGameEnd()
    {
        Debug.Log("Player has no money left! Game Over.");

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

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
