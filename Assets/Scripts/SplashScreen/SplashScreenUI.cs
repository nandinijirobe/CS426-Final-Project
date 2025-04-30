using UnityEngine;
using UnityEngine.UI;

public class SplashScreenUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject splashMenu;
    public GameObject instructionsPanel;
    public GameObject creditsPanel;
    public GameObject StoryCanvas;
    public GameObject[] comicPages; // assign Page1, Page2, Page3 in order
    public GameObject gameplayElements;

    [Header("Navigation Buttons")]
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject mainMenuButton;

    [Header("Managers")]
    public GameTimeUiManager gameTimeManager;

    private int currentPage = 0;

    void Start()
    {
        creditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        StoryCanvas.SetActive(false);
        HideAllPages();
    }

    public void StartGame()
    {
        splashMenu.SetActive(false);
        gameplayElements.SetActive(true);
        gameTimeManager.isActive = true;
    }

    public void ShowInstructions() => instructionsPanel.SetActive(true);
    public void HideInstructions() => instructionsPanel.SetActive(false);
    public void ShowCredits() => creditsPanel.SetActive(true);
    public void HideCredits() => creditsPanel.SetActive(false);

    public void ShowStory()
    {
        StoryCanvas.SetActive(true);
        currentPage = 0;
        UpdateComicView();
    }

    public void HideStory()
    {
        StoryCanvas.SetActive(false);
        HideAllPages();
    }

    public void NextPage()
    {
        if (currentPage < comicPages.Length - 1)
        {
            currentPage++;
            UpdateComicView();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateComicView();
        }
    }

    private void UpdateComicView()
    {
        HideAllPages();
        comicPages[currentPage].SetActive(true);

        prevButton.SetActive(currentPage > 0);
        nextButton.SetActive(currentPage < comicPages.Length - 1);
        mainMenuButton.SetActive(true); // Always show
    }

    private void HideAllPages()
    {
        foreach (var page in comicPages)
            page.SetActive(false);
    }
}
