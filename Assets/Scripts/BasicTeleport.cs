using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class BasicTeleport : MonoBehaviour
{
    public Transform target = null;
    public PlayerMoneyManager moneyManager;
    public GameObject player;
    public Canvas taxiCanvas;
    public float proximityDistance = 5f;

    [Header("Penalty Flash UI")]
    public CanvasGroup penaltyCanvasGroup;  // CanvasGroup on the flash image
    public float penaltyDisplayTime = 2f;

    private void Start()
    {
        taxiCanvas.enabled = false;
    }
    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        // Debug.Log("This is the current distance away from taxi: " + distance);
        if (distance < proximityDistance)
        {
            taxiCanvas.enabled = true;
            if (Input.GetKeyDown(KeyCode.T)) { 
                player.transform.position = target.transform.position;
                GetComponent<AudioSource>().Play();
                moneyManager.DeductMoney(200);
                taxiCanvas.enabled = false;
                ShowPenaltyImage();
            }
        }
        else {
            taxiCanvas.enabled = false;
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