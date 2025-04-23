using UnityEngine;

public class SplashScreenUI : MonoBehaviour
{
    public GameObject splashMenu;         // The full UI menu
    public GameObject instructionsPanel;  // The instructions popup
    public GameObject creditsPanel;  // The instructions popup

    public GameObject gameplayElements;   // The actual game content (player, camera, etc.)
    public GameTimeUiManager gameTimeManager; // assign this in the inspector

    void Start()
    {
        creditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    public void StartGame()
    {
        splashMenu.SetActive(false);
        gameplayElements.SetActive(true);

        if (gameTimeManager != null)
            gameTimeManager.isActive = true;
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }
}
