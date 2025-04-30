using UnityEngine;
using TMPro;

public class WomanDressUI : MonoBehaviour
{
    public SkinnedMeshRenderer dressRenderer;

    public Material redMat;
    public Material lightBlueMat;
    public Material purpleMat;
    public Material darkGreenMat;
    public Material limeMat;

    public TMP_Text priceText;

    public AudioSource transactionPoint; 

    private Material selectedMat;
    private Material originalMat;

    public PlayerMoneyManager moneyManager;

    void Start()
    {
        originalMat = redMat;
        selectedMat = redMat;
        priceText.text = "Choose an outfit ($50)";
        gameObject.SetActive(false);
    }

    public void SelectLightBlue() => selectedMat = lightBlueMat;
    public void SelectPurple() => selectedMat = purpleMat;
    public void SelectDarkGreen() => selectedMat = darkGreenMat;
    public void SelectLime() => selectedMat = limeMat;
    public void SelectRed() => selectedMat = redMat;

    public void ApplyChanges()
    {
        Debug.Log("dress bought");
        dressRenderer.material = selectedMat;
        transactionPoint.Play();
        gameObject.SetActive(false);

        moneyManager.DeductMoney(100);
    }

    public void CancelChanges()
    {
        dressRenderer.material = redMat;
        gameObject.SetActive(false);
    }
}
