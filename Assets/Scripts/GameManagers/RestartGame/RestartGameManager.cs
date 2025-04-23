using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestartManager : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("RestartGame() was called!");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
