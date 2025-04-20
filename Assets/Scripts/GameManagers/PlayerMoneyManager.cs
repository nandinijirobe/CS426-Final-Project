using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoneyManager : MonoBehaviour
{
    [Header("Money Settings")]
    public float startingMoney = 500f;
    public float paparazziPenalty = 50f;

    [Header("UI")]
    public TMP_Text moneyText; // Assign this in the Inspector

    private float currentMoney;

    void Start()
    {
        currentMoney = startingMoney;
        UpdateMoneyUI();
    }

    // Call this when the player hits a paparazzi
    public void DeductMoney()
    {
        currentMoney -= paparazziPenalty;
        currentMoney = Mathf.Max(currentMoney, 0);
        UpdateMoneyUI();

        if (currentMoney == 0)
        {
            Debug.Log("You're broke! Consider ending the game or limiting actions.");
            // Add logic here if needed (e.g., Game Over screen)
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
}
