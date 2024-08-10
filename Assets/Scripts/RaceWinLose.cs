using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceWinLose : MonoBehaviour
{
    [SerializeField] private GameObject winMenu; // UI для меню победы
    [SerializeField] private GameObject loseMenu; // UI для меню проигрыша
    [SerializeField] private GameObject gameUI; // UI для игры
    [SerializeField] private GameObject telemetryUI; // UI для телеметрии

    [SerializeField] private List<Image> carImageOnResultScreen; // UI элемент для отображения изображения машины

    [SerializeField] private TMP_Text winTimeText; // TextMeshPro для отображения времени на экране победы

    public CarController carController;

    [SerializeField] private List<TMP_Text> mapNameTexts;

    [SerializeField] private int award;

    private void Start()
    {
        // Убедитесь, что меню не активно при запуске
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        UpdateSceneNameTexts();
        SetCarImage();
    }

    public void ShowWinScreen(float lapTime)
    {
        winMenu.SetActive(true); // Показываем экран победы
        winTimeText.text = FormatTime(lapTime); // Отображаем время
        gameUI.SetActive(false); // Отключаем UI игры
        telemetryUI.SetActive(false); // Отключаем UI телеметрии
        carController.Stop();
        CurrencyManager.Instance.AddCurrency(award);
    }

    public void ShowLoseScreen()
    {
        loseMenu.SetActive(true); // Показываем экран проигрыша
        gameUI.SetActive(false); // Отключаем UI игры
        telemetryUI.SetActive(false); // Отключаем UI телеметрии
        carController.Stop();
    }

    public void QuitGame()
    {
        Time.timeScale = 1; // Восстанавливаем время
        Application.Quit(); // Выйти из игры
    }

    public void RestartLevel()
    {
        Time.timeScale = 1; // Восстанавливаем время
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Перезагрузка сцены
    }

    public void SetCarImage()
    {
        if (carImageOnResultScreen != null)
        {
            foreach (var item in carImageOnResultScreen)
            {
                item.sprite = SelectionManager.SelectedCarImage;
            }
        }
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time - Mathf.Floor(time)) * 1000);

        return string.Format("{0:D2}'{1:D2}''{2:D3}", minutes, seconds, milliseconds);
    }

    private void UpdateSceneNameTexts()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        foreach (var textElement in mapNameTexts)
        {
            textElement.text = sceneName;
        }
    }
}
