using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoneyManager : MonoBehaviour
{
    [Header("Money Settings")]
    public int startingMoney = 500;

    [Header("UI")]
    public TMP_Text moneyText; // Assign this in the Inspector

    private int currentMoney;

    void Start()
    {
        currentMoney = startingMoney;
        UpdateMoneyUI();
    }

    // Call this when the player hits a paparazzi
    public void DeductMoney(int amount)
    {
        currentMoney -= amount;
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

    public void AddMoney(int amount) 
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }
}
