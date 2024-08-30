using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectionManager : MonoBehaviour
{
    // Предположим, что вы инициализируете эти данные в зависимости от платформы
    private ICarDataManager carDataManager;
    private ITrackDataManager trackDataManager;

    [Header("Car Selection")]
    [SerializeField] private List<CarData> carDataList;
    [SerializeField] private Image carImage;
    [SerializeField] private TMP_Text carNameText;

    [Header("Car Parameters Display")]
    [SerializeField] private Image speedIndicator;
    [SerializeField] private Image handlingIndicator;
    [SerializeField] private Image powerIndicator;

    [SerializeField] private Button nextCarButton;
    [SerializeField] private Button previousCarButton;

    [Header("Track Selection")]
    [SerializeField] private List<TrackData> trackDataList;
    [SerializeField] private Image trackImage;
    [SerializeField] private TMP_Text trackNameText;
    [SerializeField] private Button nextTrackButton;
    [SerializeField] private Button previousTrackButton;

    [Header("Car Unlocking")]
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private GameObject lockOverlay;

    [Header("Buttons")]
    [SerializeField] private Button startRaceButton;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;

    private int selectedCarIndex;
    private int selectedTrackIndex;
    private bool isCarChanging = false;
    private bool isTrackChanging = false;

    public static Sprite SelectedCarImage;

    private void Awake()
    {
        carDataManager = CarDataFactory.CreateCarDataManager();
        trackDataManager = TrackDataFactory.CreateTrackDataManager();
    }

    private void Start()
    {
        

        carDataManager.LoadCarData(carDataList, out selectedCarIndex);
        trackDataManager.LoadTrackData(out selectedTrackIndex);
        UpdateCarSelection();
        UpdateTrackSelection();

        nextCarButton.onClick.AddListener(NextCar);
        previousCarButton.onClick.AddListener(PreviousCar);
        nextTrackButton.onClick.AddListener(NextTrack);
        previousTrackButton.onClick.AddListener(PreviousTrack);
        startRaceButton.onClick.AddListener(StartRace);
        purchaseButton.onClick.AddListener(PurchaseCar);

        Time.timeScale = 1.0f;
    }

    private void NextCar()
    {
        if (isCarChanging) return;

        selectedCarIndex = (selectedCarIndex + 1) % carDataList.Count;
        StartCoroutine(SmoothCarChange());
        carDataManager.SaveCarData(selectedCarIndex, carDataList);
    }

    private void PreviousCar()
    {
        if (isCarChanging) return;

        selectedCarIndex = (selectedCarIndex - 1 + carDataList.Count) % carDataList.Count;
        StartCoroutine(SmoothCarChange());
        carDataManager.SaveCarData(selectedCarIndex, carDataList);
    }

    private void NextTrack()
    {
        if (isTrackChanging) return;

        selectedTrackIndex = (selectedTrackIndex + 1) % trackDataList.Count;
        StartCoroutine(SmoothTrackChange());
        trackDataManager.SaveTrackData(selectedTrackIndex);
    }

    private void PreviousTrack()
    {
        if (isTrackChanging) return;

        selectedTrackIndex = (selectedTrackIndex - 1 + trackDataList.Count) % trackDataList.Count;
        StartCoroutine(SmoothTrackChange());
        trackDataManager.SaveTrackData(selectedTrackIndex);
    }

    private IEnumerator SmoothCarChange()
    {
        if (carImage == null) yield break;

        isCarChanging = true;

        yield return StartCoroutine(FadeOut(carImage, animationDuration));
        UpdateCarSelection();
        yield return StartCoroutine(FadeIn(carImage, animationDuration));

        isCarChanging = false;
    }

    private IEnumerator SmoothTrackChange()
    {
        if (trackImage == null) yield break;

        isTrackChanging = true;

        yield return StartCoroutine(FadeOut(trackImage, animationDuration));
        UpdateTrackSelection();
        yield return StartCoroutine(FadeIn(trackImage, animationDuration));

        isTrackChanging = false;
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        if (image == null) yield break;

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
        if (image == null) yield break;

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
        if (carDataList.Count == 0) return;

        var selectedCar = carDataList[selectedCarIndex];
        if (carImage.sprite == selectedCar.carImage && carNameText.text == selectedCar.carPrefab.name) return;

        carImage.sprite = selectedCar.carImage;
        carNameText.text = selectedCar.carPrefab.name;

        UpdateIndicator(speedIndicator, selectedCar.carSpeed);
        UpdateIndicator(handlingIndicator, selectedCar.carHandling);
        UpdateIndicator(powerIndicator, selectedCar.carPower);

        purchaseButton.gameObject.SetActive(!selectedCar.isUnlocked);
        startRaceButton.gameObject.SetActive(selectedCar.isUnlocked);
        lockOverlay.SetActive(!selectedCar.isUnlocked);
        priceText.text = selectedCar.isUnlocked ? string.Empty : selectedCar.carPrice.ToString();
        if (!selectedCar.isUnlocked) carImage.sprite = selectedCar.carLockImage;
    }

    private void UpdateTrackSelection()
    {
        if (trackDataList.Count == 0) return;

        var selectedTrack = trackDataList[selectedTrackIndex];
        if (trackImage.sprite == selectedTrack.trackImage && trackNameText.text == selectedTrack.trackSceneName) return;

        trackImage.sprite = selectedTrack.trackImage;
        trackNameText.text = selectedTrack.trackSceneName;
    }

    private void UpdateIndicator(Image indicator, float value)
    {
        float maxValue = 100f;
        RectTransform rectTransform = indicator.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float normalizedValue = Mathf.Clamp01(value / maxValue);
            rectTransform.sizeDelta = new Vector2(normalizedValue * 100, rectTransform.sizeDelta.y);
        }
    }

    private void PurchaseCar()
    {
        var selectedCar = carDataList[selectedCarIndex];
        int carPrice = selectedCar.carPrice;

        if (CurrencyManager.Instance.GetCurrency() >= carPrice)
        {
            CurrencyManager.Instance.SpendCurrency(carPrice);
            selectedCar.isUnlocked = true;
            carDataManager.SaveCarData(selectedCarIndex, carDataList);
            UpdateCarSelection();
            if (!isCarChanging)
            {
                StartCoroutine(SmoothCarChange());
            }
        }
    }

    public Sprite GetSelectedCarImage()
    {
        return carDataList[selectedCarIndex].carImage;
    }

    private void StartRace()
    {
        var selectedCar = carDataList[selectedCarIndex];
        if (!selectedCar.isUnlocked) return;

        SelectedCarImage = selectedCar.carImage;

        GameData.SelectedCarName = selectedCar.carPrefab.name;
        GameData.SelectedTrack = trackDataList[selectedTrackIndex].trackSceneName;

        SceneManager.LoadScene(trackDataList[selectedTrackIndex].trackSceneName);
    }
}
