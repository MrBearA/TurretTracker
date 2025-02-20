using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    public GameObject gameOverUI;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowGameOverScreen()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // PAUSE THE GAME
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // RESUME TIME BEFORE RESTARTING
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
