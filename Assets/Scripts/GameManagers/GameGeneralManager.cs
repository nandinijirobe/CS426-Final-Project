using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGeneralManager : MonoBehaviour
{
    int cash;
    int numAuditions = 0;
    public TMP_Text auditionCount;
    public PlayerMoneyManager moneyManager;
    public BayesianDecisionManager decisionManager; // Assign this in Inspector

    public GlowToggle glowToggle;

    [Header("Win UI")]
    public GameObject winScreenUI; // Assign this in the Inspector
    private int auditionsToWin = 5;

    [Header("Game Restart")]
    public float restartDelay = 5f; // Time in seconds before the game restarts after win

    public bool hasWon = false;

        void Start()
    {
        // Ensure win screen is hidden on game start
        if (winScreenUI != null)
        {
            winScreenUI.SetActive(false);
        }

        auditionCount.text = "Auditions: 0/" + auditionsToWin;
    }

    public void updateAuditionCount()
    {
        if (hasWon) return;

        if (numAuditions < auditionsToWin)
        {
            numAuditions++;
            cash += 100;

            // auditionCount.text = "Auditions: " + numAuditions + "/5";
            moneyManager.AddMoney(100);
            glowToggle.TriggerGlow();
            auditionCount.text = "Auditions: " + numAuditions + "/" + auditionsToWin;

            // 🟢 Update fame level proportionally (0.2 per audition)
            if (decisionManager != null)
            {
                decisionManager.fameLevel = Mathf.Clamp01(numAuditions / (float)auditionsToWin);
            }
        }

        if (numAuditions >= auditionsToWin)
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        hasWon = true;
        if (winScreenUI != null)
        {
            winScreenUI.SetActive(true);
        }

        Debug.Log("You’ve won the game!");

        StartCoroutine(RestartAfterDelay());
    }

    System.Collections.IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSecondsRealtime(restartDelay);

        Time.timeScale = 1f; // Unpause the game
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
