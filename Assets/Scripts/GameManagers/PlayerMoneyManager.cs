using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerMoneyManager : MonoBehaviour
{
    [Header("Money Settings")]
    public int startingMoney = 500;

    [Header("UI")]
    public TMP_Text moneyText; // Assign this in the Inspector
    public CanvasGroup clothingCanvasGroup; // to show -$75 when buying clothes
    public CanvasGroup auditionCanvasGroup; // to show +$100 when getting audition
    public float moneyDeductionDisplayTime = 1f;

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

        if (amount == 75)
        {
            ShowMoneyImage(clothingCanvasGroup);
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

        if (amount == 100)
        {
            ShowMoneyImage(auditionCanvasGroup);
        }
    }

    void ShowMoneyImage(CanvasGroup canvasGroup)
    {
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.8f;
            canvasGroup.gameObject.SetActive(true);
            StartCoroutine(FadeOutMoneyImage(canvasGroup));
        }
    }

    IEnumerator FadeOutMoneyImage(CanvasGroup canvasGroup)
    {
        float timer = 0f;
        float startAlpha = canvasGroup.alpha;

        while (timer < moneyDeductionDisplayTime)
        {
            timer += Time.deltaTime;
            float t = timer / moneyDeductionDisplayTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }
}
