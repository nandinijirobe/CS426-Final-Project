using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGeneralManager : MonoBehaviour
{
    int numAuditions = 0;
    public TMP_Text auditionCount;

    [Header("Win UI")]
    public GameObject winScreenUI; // Assign this in the Inspector
    public int auditionsToWin = 5;

    [Header("Game Restart")]
    public float restartDelay = 5f; // Time in seconds before the game restarts after win

    private bool hasWon = false;

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
            auditionCount.text = "Auditions: " + numAuditions + "/" + auditionsToWin;
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
