using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectionManager : MonoBehaviour
{
    [Header("Car Selection")]
    [SerializeField] private List<CarData> carDataList; // Список данных о машинах

    [SerializeField] private Image carImage; // Изображение для отображения текущей выбранной машины
    [SerializeField] private TMP_Text carNameText; // Текст для отображения названия машины

    [Header("Car Parameters Display")]
    [SerializeField] private Image speedIndicator; // Изображение для отображения скорости
    [SerializeField] private Image handlingIndicator; // Изображение для отображения управляемости
    [SerializeField] private Image powerIndicator; // Изображение для отображения мощности

    [SerializeField] private Button nextCarButton; // Кнопка для выбора следующей машины
    [SerializeField] private Button previousCarButton; // Кнопка для выбора предыдущей машины

    [Header("Track Selection")]
    [SerializeField] private List<TrackData> trackDataList; // Список данных о треках

    [SerializeField] private Image trackImage; // Изображение для отображения текущей выбранной трассы
    [SerializeField] private TMP_Text trackNameText; // Текст для отображения названия трассы
    [SerializeField] private Button nextTrackButton; // Кнопка для выбора следующей трассы
    [SerializeField] private Button previousTrackButton; // Кнопка для выбора предыдущей трассы

    [Header("Car Unlocking")]
    [SerializeField] private Button purchaseButton; // Кнопка для покупки машины
    [SerializeField] private TMP_Text priceText; // Текст на кнопке покупки
    [SerializeField] private GameObject lockOverlay; // Замок или затемненное изображение для отображения блокировки

    [Header("Buttons")]
    [SerializeField] private Button startRaceButton; // Кнопка для начала гонки

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f; // Длительность анимации

    private int selectedCarIndex = 0;
    private int selectedTrackIndex = 0;
    private bool isCarChanging = false; // Флаг для контроля анимации смены машины

    private const string UNLOCKED_CARS_KEY = "UnlockedCars"; // Ключ для хранения статусов разблокировки машин в PlayerPrefs

    private void Start()
    {
        // Проверяем, что списки не пустые
        if (carDataList.Count == 0 || trackDataList.Count == 0)
        {
            Debug.LogError("One or more lists are empty. Please assign the lists in the inspector.");
            return;
        }

        // Загрузка сохраненных данных
        LoadCarData();

        // Инициализация отображения
        UpdateCarSelection();
        UpdateTrackSelection();

        // Подключаем обработчики событий
        nextCarButton.onClick.AddListener(NextCar);
        previousCarButton.onClick.AddListener(PreviousCar);
        nextTrackButton.onClick.AddListener(NextTrack);
        previousTrackButton.onClick.AddListener(PreviousTrack);
        startRaceButton.onClick.AddListener(StartRace);
        purchaseButton.onClick.AddListener(PurchaseCar);
    }

    private void NextCar()
    {
        selectedCarIndex = (selectedCarIndex + 1) % carDataList.Count;
        if (!isCarChanging) // Проверяем флаг, чтобы предотвратить повторный вызов
        {
            StartCoroutine(SmoothCarChange());
        }
        else
        {
            UpdateCarSelection(); // Обновляем выбор машины без анимации
        }
    }

    private void PreviousCar()
    {
        selectedCarIndex = (selectedCarIndex - 1 + carDataList.Count) % carDataList.Count;
        if (!isCarChanging) // Проверяем флаг, чтобы предотвратить повторный вызов
        {
            StartCoroutine(SmoothCarChange());
        }
        else
        {
            UpdateCarSelection(); // Обновляем выбор машины без анимации
        }
    }

    private void NextTrack()
    {
        selectedTrackIndex = (selectedTrackIndex + 1) % trackDataList.Count;
        StartCoroutine(SmoothTrackChange());
    }

    private void PreviousTrack()
    {
        selectedTrackIndex = (selectedTrackIndex - 1 + trackDataList.Count) % trackDataList.Count;
        StartCoroutine(SmoothTrackChange());
    }

    private IEnumerator SmoothCarChange()
    {
        isCarChanging = true; // Устанавливаем флаг, что анимация происходит
        yield return StartCoroutine(FadeOut(carImage, animationDuration)); // Плавное исчезновение старого изображения
        UpdateCarSelection(); // Обновляем выбор машины
        yield return StartCoroutine(FadeIn(carImage, animationDuration)); // Плавное появление нового изображения
        isCarChanging = false; // Сбрасываем флаг после завершения анимации
    }

    private IEnumerator SmoothTrackChange()
    {
        yield return StartCoroutine(FadeOut(trackImage, animationDuration)); // Плавное исчезновение старого изображения
        UpdateTrackSelection(); // Обновляем выбор трассы
        yield return StartCoroutine(FadeIn(trackImage, animationDuration)); // Плавное появление нового изображения
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = endColor;
    }

    private IEnumerator FadeIn(Image image, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = new Color(image.color.r, image.color.g, image.color.b, 0f);
        Color endColor = new Color(image.color.r, image.color.g, image.color.b, 1f);

        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = endColor;
    }

    private void UpdateCarSelection()
    {
        var selectedCar = carDataList[selectedCarIndex];
        carImage.sprite = selectedCar.carImage;
        carNameText.text = selectedCar.carPrefab.name;

        // Обновляем индикаторы
        UpdateIndicator(speedIndicator, selectedCar.carSpeed);
        UpdateIndicator(handlingIndicator, selectedCar.carHandling);
        UpdateIndicator(powerIndicator, selectedCar.carPower);

        // Проверка, разблокирована ли машина
        if (selectedCar.isUnlocked)
        {
            purchaseButton.gameObject.SetActive(false); // Скрываем кнопку покупки
            startRaceButton.gameObject.SetActive(true);
            lockOverlay.SetActive(false); // Скрываем замок
        }
        else
        {
            purchaseButton.gameObject.SetActive(true); // Показываем кнопку покупки
            startRaceButton.gameObject.SetActive(false);
            priceText.text = selectedCar.carPrice.ToString(); // Отображаем цену
            lockOverlay.SetActive(true); // Показываем замок
            carImage.sprite = selectedCar.carLockImage;
        }
    }

    private void UpdateTrackSelection()
    {
        var selectedTrack = trackDataList[selectedTrackIndex];
        trackImage.sprite = selectedTrack.trackImage;
        trackNameText.text = selectedTrack.trackSceneName;
    }

    private void UpdateIndicator(Image indicator, float value)
    {
        // Максимальное значение для нормализации
        float maxValue = 100f;

        // Пропорционально изменяем ширину изображения
        RectTransform rectTransform = indicator.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float normalizedValue = Mathf.Clamp01(value / maxValue);
            rectTransform.sizeDelta = new Vector2(normalizedValue * 100, rectTransform.sizeDelta.y); // 100 - максимальная ширина индикатора
        }
    }

    private void PurchaseCar()
    {
        var selectedCar = carDataList[selectedCarIndex];
        int carPrice = selectedCar.carPrice;

        // Проверяем, есть ли у игрока достаточно валюты для покупки
        if (CurrencyManager.Instance.GetCurrency() >= carPrice)
        {
            CurrencyManager.Instance.SpendCurrency(carPrice); // Списываем валюту
            selectedCar.isUnlocked = true; // Разблокируем машину
            SaveCarData(); // Сохраняем статус купленных машин
            UpdateCarSelection(); // Обновляем интерфейс
            if (!isCarChanging) // Если анимация не происходит
            {
                StartCoroutine(SmoothCarChange()); // Запускаем плавное изменение изображения после разблокировки
            }
        }
        else
        {
            Debug.Log("Not enough currency to purchase this car.");
        }
    }

    private void StartRace()
    {
        var selectedCar = carDataList[selectedCarIndex];
        if (!selectedCar.isUnlocked)
        {
            Debug.Log("This car is locked. Please purchase it first.");
            return;
        }

        // Сохраняем выбранные значения в GameData
        GameData.SelectedCarName = selectedCar.carPrefab.name;
        GameData.SelectedTrack = trackDataList[selectedTrackIndex].trackSceneName;

        // Загружаем выбранную трассу
        SceneManager.LoadScene(trackDataList[selectedTrackIndex].trackSceneName);
    }

    // Сохранение данных о купленных машинах
    private void SaveCarData()
    {
        for (int i = 0; i < carDataList.Count; i++)
        {
            PlayerPrefs.SetInt(UNLOCKED_CARS_KEY + i, carDataList[i].isUnlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    // Загрузка данных о купленных машинах
    private void LoadCarData()
    {
        for (int i = 0; i < carDataList.Count; i++)
        {
            carDataList[i].isUnlocked = PlayerPrefs.GetInt(UNLOCKED_CARS_KEY + i, 0) == 1;
        }
    }
}
