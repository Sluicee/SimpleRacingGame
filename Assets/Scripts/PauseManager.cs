using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // UI для меню паузы
    [SerializeField] private GameObject gameUI;    // UI для игрового интерфейса
    private bool isPaused = false;
    private CarController carController;

    private void Start()
    {
        carController = FindObjectOfType<CarController>();
    }

    public void Pause()
    {
        if (isPaused) return;

        // Немедленно выключаем звук двигателя
        carController.engineSource.Pause();

        // Останавливаем время и показываем меню паузы
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameUI.SetActive(false);
        isPaused = true;
        PauseTrafficLight();
    }

    public void Resume()
    {
        if (!isPaused) return;

        // Восстанавливаем время
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameUI.SetActive(true);
        isPaused = false;

        carController.engineSource.UnPause();
        ResumeTrafficLight();
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
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
