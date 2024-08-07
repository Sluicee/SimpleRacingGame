using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectionManager : MonoBehaviour
{
    [Header("Car Selection")]
    [SerializeField] private List<GameObject> carPrefabs; // Список префабов машин
    [SerializeField] private List<Sprite> carImages; // Список изображений машин
    [SerializeField] private List<float> carSpeeds; // Список скоростей машин
    [SerializeField] private List<float> carHandling; // Список управляемостей машин
    [SerializeField] private List<float> carPower; // Список мощностей машин

    [SerializeField] private Image carImage; // Изображение для отображения текущей выбранной машины
    [SerializeField] private TMP_Text carNameText; // Текст для отображения названия машины

    [Header("Car Parameters Display")]
    [SerializeField] private Image speedIndicator; // Изображение для отображения скорости (изменяет ширину)
    [SerializeField] private Image handlingIndicator; // Изображение для отображения управляемости (изменяет ширину)
    [SerializeField] private Image powerIndicator; // Изображение для отображения мощности (изменяет ширину)

    [SerializeField] private Button nextCarButton; // Кнопка для выбора следующей машины
    [SerializeField] private Button previousCarButton; // Кнопка для выбора предыдущей машины

    [Header("Track Selection")]
    [SerializeField] private List<string> trackScenes; // Список названий сцен с трассами
    [SerializeField] private List<Sprite> trackImages; // Список изображений трасс

    [SerializeField] private Image trackImage; // Изображение для отображения текущей выбранной трассы
    [SerializeField] private TMP_Text trackNameText; // Текст для отображения названия трассы
    [SerializeField] private Button nextTrackButton; // Кнопка для выбора следующей трассы
    [SerializeField] private Button previousTrackButton; // Кнопка для выбора предыдущей трассы

    [Header("Buttons")]
    [SerializeField] private Button startRaceButton; // Кнопка для начала гонки

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f; // Длительность анимации

    private int selectedCarIndex = 0;
    private int selectedTrackIndex = 0;

    private void Start()
    {
        // Проверяем, что списки не пустые
        if (carPrefabs.Count == 0 || carImages.Count == 0 || trackScenes.Count == 0 || trackImages.Count == 0)
        {
            Debug.LogError("One or more lists are empty. Please assign the lists in the inspector.");
            return;
        }

        // Инициализация отображения
        UpdateCarSelection();
        UpdateTrackSelection();

        // Подключаем обработчики событий
        nextCarButton.onClick.AddListener(NextCar);
        previousCarButton.onClick.AddListener(PreviousCar);
        nextTrackButton.onClick.AddListener(NextTrack);
        previousTrackButton.onClick.AddListener(PreviousTrack);
        startRaceButton.onClick.AddListener(StartRace);
    }

    private void NextCar()
    {
        selectedCarIndex = (selectedCarIndex + 1) % carPrefabs.Count;
        StartCoroutine(SmoothCarChange());
    }

    private void PreviousCar()
    {
        selectedCarIndex = (selectedCarIndex - 1 + carPrefabs.Count) % carPrefabs.Count;
        StartCoroutine(SmoothCarChange());
    }

    private void NextTrack()
    {
        selectedTrackIndex = (selectedTrackIndex + 1) % trackScenes.Count;
        StartCoroutine(SmoothTrackChange());
    }

    private void PreviousTrack()
    {
        selectedTrackIndex = (selectedTrackIndex - 1 + trackScenes.Count) % trackScenes.Count;
        StartCoroutine(SmoothTrackChange());
    }

    private IEnumerator SmoothCarChange()
    {
        yield return StartCoroutine(FadeOut(carImage, animationDuration)); // Плавное исчезновение старого изображения
        UpdateCarSelection(); // Обновляем выбор машины
        yield return StartCoroutine(FadeIn(carImage, animationDuration)); // Плавное появление нового изображения
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
        carImage.sprite = carImages[selectedCarIndex];
        carNameText.text = carPrefabs[selectedCarIndex].name;

        // Обновляем индикаторы
        UpdateIndicator(speedIndicator, carSpeeds[selectedCarIndex]);
        UpdateIndicator(handlingIndicator, carHandling[selectedCarIndex]);
        UpdateIndicator(powerIndicator, carPower[selectedCarIndex]);
    }

    private void UpdateTrackSelection()
    {
        trackImage.sprite = trackImages[selectedTrackIndex];
        trackNameText.text = trackScenes[selectedTrackIndex];
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

    private void StartRace()
    {
        // Сохраняем выбранные значения в GameData
        GameData.SelectedCarName = carPrefabs[selectedCarIndex].name;
        GameData.SelectedTrack = trackScenes[selectedTrackIndex];

        // Загружаем выбранную трассу
        SceneManager.LoadScene(trackScenes[selectedTrackIndex]);
    }

}
