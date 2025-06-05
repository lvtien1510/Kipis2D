using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneTransitionManager.Instance.TransitionToScene(currentScene.name);
    }

    public void ReturnToHome()
    {
        Time.timeScale = 1f;
        SceneTransitionManager.Instance.TransitionToScene("Home");
    }
}
