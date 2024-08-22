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

    [Header ("Record info")]

    [SerializeField] private TextMeshProUGUI recordTimeText; // Текст для отображения рекордного времени
    [SerializeField] private TextMeshProUGUI timeDifferenceText; // Текст для отображения разницы во времени
    [SerializeField] private GameObject newRecordText; // Текст "Новый рекрод"
    [SerializeField] private GameObject prevRecordText; // Текст "Прошлый рекрод"

    [SerializeField] private Color fasterTimeColor = Color.green; // Цвет, если время быстрее рекорда
    [SerializeField] private Color slowerTimeColor = Color.red;   // Цвет, если время медленнее рекорда

    private string trackName;  // Имя текущей трассы (уникальное)

    private void Start()
    {
        // Убедитесь, что меню не активно при запуске
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        UpdateSceneNameTexts();
        SetCarImage();
        trackName = SceneManager.GetActiveScene().name; // Установите имя трассы здесь или передавайте его в скрипт
        LoadRecordTime();
    }

    public void ShowWinScreen(float lapTime)
    {
        winMenu.SetActive(true); // Показываем экран победы
        winTimeText.text = FormatTime(lapTime); // Отображаем время
        gameUI.SetActive(false); // Отключаем UI игры
        telemetryUI.SetActive(false); // Отключаем UI телеметрии

        float recordTime = GetRecordTime();

        // Проверяем, если время круга лучше, чем рекорд
        if (lapTime < recordTime)
        {
            SaveRecordTime(lapTime);
            newRecordText.SetActive(true);
            Debug.Log("New record! Time: " + FormatTime(lapTime));
        }
        else
        {
            newRecordText.SetActive(false);
            Debug.Log("Your time: " + FormatTime(lapTime));
        }

        UpdateTimeDifferenceText(lapTime, recordTime);

        carController.SetBrakeAcceleration(100f);
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

    private float GetRecordTime()
    {
        // Загрузка рекордного времени для текущей трассы
        return PlayerPrefs.GetFloat(trackName + "_RecordTime", Mathf.Infinity);
    }

    private void SaveRecordTime(float newRecord)
    {
        // Сохранение нового рекордного времени для текущей трассы
        PlayerPrefs.SetFloat(trackName + "_RecordTime", newRecord);
        PlayerPrefs.Save();
    }

    private void LoadRecordTime()
    {
        // Загрузка и отображение рекордного времени
        float recordTime = GetRecordTime();
        if (recordTime != Mathf.Infinity)
        {
            recordTimeText.text = FormatTime(recordTime);
            prevRecordText.SetActive(true);
        }
        else
        {
            recordTimeText.text = "";
            prevRecordText.SetActive(false);
            Debug.Log("No record time set yet.");
        }
    }

    private void UpdateTimeDifferenceText(float lapTime, float recordTime)
    {
        // Вычисляем разницу во времени
        if (recordTime != Mathf.Infinity)
        {
            float timeDifference = lapTime - recordTime;
            string formattedDifference = FormatTime(Mathf.Abs(timeDifference));

            // Меняем текст и цвет в зависимости от того, лучше ли текущее время рекорда
            if (timeDifference < 0)
            {
                timeDifferenceText.color = fasterTimeColor;
                timeDifferenceText.text = "-" + formattedDifference; // Время лучше
            }
            else
            {
                timeDifferenceText.color = slowerTimeColor;
                timeDifferenceText.text = "+" + formattedDifference; // Время хуже
            }
        }
        else
        {
            // Если рекорда нет, не отображаем разницу
            timeDifferenceText.text = "";
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
