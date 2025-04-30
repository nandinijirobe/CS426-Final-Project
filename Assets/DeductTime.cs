using System;
using System.Collections;
using UnityEngine;

public class DeductTime : MonoBehaviour
{
    public AudioClip errorSound; 
    private AudioSource audioSource;

    public GameTimeUiManager gameTimeUiManager;

    [Header("Penalty Flash UI")]
    public CanvasGroup penaltyCanvasGroup;  // CanvasGroup on the flash image
    public float penaltyDisplayTime = 2f;

    public OutfitChanger outfit_changer;

    private bool disguiseOn;

    private void Start()
    {
        disguiseOn = outfit_changer != null ? outfit_changer.disguiseOn : false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (outfit_changer != null)
        {
            disguiseOn = outfit_changer.disguiseOn;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && (outfit_changer == null || !outfit_changer.disguiseOn))
        {
            if (errorSound != null)
            {
                audioSource.clip = errorSound;
                gameTimeUiManager.totalMinutesInDay -= 30;
                audioSource.Play();
                ShowPenaltyImage();
            }
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
