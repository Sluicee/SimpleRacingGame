using UnityEngine;
using UnityEngine.SceneManagement;
#if YANDEX_SDK
using YG;
#endif


public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // UI для меню паузы
    [SerializeField] private GameObject gameUI;    // UI для игрового интерфейса
    [SerializeField] private MusicManager musicManager;
    private bool isPaused = false;
    private CarController carController;

    private void Start()
    {
        #if YANDEX_SDK
            YandexGame.GameplayStart();
        #endif
        carController = FindObjectOfType<CarController>();
    }

    public void Pause()
    {
        if (isPaused) return;

#if YANDEX_SDK
        YandexGame.GameplayStop();
#endif

        // Немедленно выключаем звук двигателя
        carController.engineSource.Pause();

        musicManager.EnterMenuMode(); // Измените громкость при паузе

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

#if YANDEX_SDK
        YandexGame.GameplayStart();
#endif

        musicManager.EnterGameMode(); // Восстановите громкость в игре

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
