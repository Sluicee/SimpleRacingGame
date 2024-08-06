using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // UI для меню паузы
    [SerializeField] private GameObject gameUI; // UI для меню паузы
    private bool isPaused = false;

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameUI.SetActive(false);
        isPaused = true;
        PauseTrafficLight(); // Приостановка работы светофора
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameUI.SetActive(true);
        isPaused = false;
        ResumeTrafficLight(); // Возобновление работы светофора
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        // Выйти из игры
        Application.Quit();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PauseTrafficLight()
    {
        var trafficLight = FindObjectOfType<RaceTrafficLight>();
        if (trafficLight != null)
        {
            trafficLight.PauseTrafficLight();
        }
    }

    private void ResumeTrafficLight()
    {
        var trafficLight = FindObjectOfType<RaceTrafficLight>();
        if (trafficLight != null)
        {
            trafficLight.ResumeTrafficLight();
        }
    }
}
