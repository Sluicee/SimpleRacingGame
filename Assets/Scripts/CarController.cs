using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour
{
    [SerializeField] private float acceleration = 5f;       // Ускорение машины
    [SerializeField] private float maxSpeed = 20f;          // Максимальная скорость машины
    [SerializeField] private float brakeAcceleration = 10f; // Тормозное ускорение
    [SerializeField] private float boostMultiplier = 2f;    // Множитель ускорения при бусте
    [SerializeField] private float boostDuration = 5f;      // Длительность буста
    [SerializeField] private float boostEffectSpeed = 5f;      // Длительность буста
    [SerializeField] private float decelerationRate = 2f;    // Скорость уменьшения максимальной скорости после буста

    [SerializeField] private TextMeshProUGUI speedText;     // Ссылка на TextMeshPro текст для отображения скорости
    [SerializeField] private TextMeshProUGUI lapTimeText;   // Ссылка на TextMeshPro текст для отображения времени круга
    [SerializeField] private RectTransform speedIndicator;  // Ссылка на RectTransform для отображения текущей скорости

    [SerializeField] private RaceWinLose raceWinLose; // Ссылка на RaceWinLose

    [SerializeField] private CameraFollow cameraFollow; // Ссылка на CameraFollow

    [SerializeField] private GameObject boostEffect; // Ссылка на материал с шейдером

    private float tempBoostEffectSpeed;

    public float currentSpeed = 0f;       // Текущая скорость машины
    private float currentAcceleration;    // Текущее ускорение машины
    private bool isBoosting = false;      // Флаг активации буста
    private bool hasBoosted = false;      // Флаг, указывающий, что буст уже был использован
    private float boostEndTime = 0f;      // Время окончания буста

    private Rigidbody rb;

    private float lapStartTime;           // Время начала круга
    private float lapEndTime;             // Время окончания круга
    private bool lapStarted = false;      // Флаг начала круга
    private bool lapEnded = false;        // Флаг окончания круга

    private float tempMaxSpeed;           // Храним начальное значение максимальной скорости
    private float targetMaxSpeed;         // Целевая максимальная скорость при плавном снижении
    private bool isDecelerating = false;  // Флаг, указывающий на процесс плавного снижения скорости
    private bool isAccelerating = false;

    public AudioSource engineSource;

    public float fadeOutDuration { get; private set; } = 0.5f;
    public float fadeInDuration { get; private set; } = 0.5f;

    private float initialVolume; // Начальная громкость звука

    private bool canMove = false; // New flag to control car movement

    public void Initialize(TextMeshProUGUI _speedText, TextMeshProUGUI _lapTimeText, RectTransform _speedIndicator, RaceWinLose _raceWinLose, CameraFollow _cameraFollow, GameObject _boostEffect)
    {
        speedText = _speedText;
        lapTimeText = _lapTimeText;
        speedIndicator = _speedIndicator;
        raceWinLose = _raceWinLose;
        cameraFollow = _cameraFollow;
        boostEffect = _boostEffect;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentAcceleration = acceleration; // Устанавливаем текущее ускорение в начальное значение
        lapTimeText.text = "00'00' '000"; // Инициализируем текст времени круга

        tempMaxSpeed = maxSpeed;  // Сохраняем начальное значение максимальной скорости

        // Сохраняем начальное значение громкости
        initialVolume = engineSource.volume;

        // Установка начального значения для speedIndicator
        if (speedIndicator != null)
        {
            speedIndicator.pivot = new Vector2(0f, 0f); // Установка якоря снизу
            speedIndicator.sizeDelta = new Vector2(speedIndicator.sizeDelta.x, 0f); // Начальная высота = 0
        }
    }

    void FixedUpdate()
    {
        // Управление ускорением
        if (isAccelerating && canMove)
        {
            currentSpeed += currentAcceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= brakeAcceleration * Time.deltaTime;
        }

        // Ограничение скорости
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // Применение ускорения
        Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Управление бустом
        //if (Input.GetKeyDown(KeyCode.Space) && !isBoosting && !hasBoosted && canMove)
        //{
        //    StartBoost();
        //}

        if (isBoosting && Time.time > boostEndTime)
        {
            EndBoost();
        }

        if (!isBoosting && isDecelerating)
        {
            // Плавное снижение максимальной скорости после окончания буста
            maxSpeed = Mathf.Lerp(maxSpeed, targetMaxSpeed, decelerationRate * Time.deltaTime);

            if (Mathf.Abs(maxSpeed - targetMaxSpeed) < 0.01f)
            {
                maxSpeed = targetMaxSpeed;
                isDecelerating = false;
            }
        }

        // Обновление TextMeshPro текста с текущей скоростью
        if (speedText != null)
        {
            speedText.text = Mathf.RoundToInt(currentSpeed * 10).ToString(); // Умножаем на 10 для более реалистичных значений скорости
        }

        // Обновление времени круга
        if (lapStarted && !lapEnded)
        {
            float lapTime = Time.time - lapStartTime;
            lapTimeText.text = FormatTime(lapTime);
        }

        // Обновление отображения скорости
        UpdateSpeedIndicator();
    }

    public void StartBoost()
    {
        if (lapStarted)
        {
            isBoosting = true;
            currentAcceleration = acceleration * boostMultiplier; // Увеличиваем ускорение
            boostEndTime = Time.time + boostDuration;
            hasBoosted = true; // Устанавливаем флаг использования буста
            targetMaxSpeed = maxSpeed;  // Сохраняем текущую максимальную скорость как цель
            maxSpeed *= 2; // Увеличиваем максимальную скорость вдвое
            isDecelerating = false; // Сбрасываем флаг, если буст активен
            tempBoostEffectSpeed = cameraFollow.GetFollowSpeed;
            cameraFollow.SetFollowSpeed(boostEffectSpeed);
            boostEffect.SetActive(true);
            Debug.Log("Boost activated.");
        }
    }

    private void EndBoost()
    {
        isBoosting = false;
        currentAcceleration = acceleration; // Возвращаемся к нормальному ускорению
        targetMaxSpeed = tempMaxSpeed; // Устанавливаем цель для максимальной скорости
        isDecelerating = true; // Запускаем процесс плавного снижения скорости
        cameraFollow.SetFollowSpeed(tempBoostEffectSpeed);
        boostEffect.SetActive(false);
        Debug.Log("Boost ended.");
    }

    // Форматирование времени в минуты, секунды и миллисекунды
    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time - Mathf.Floor(time)) * 1000);

        return string.Format("{0:D2}'{1:D2}''{2:D3}", minutes, seconds, milliseconds);
    }

    public void StartLap()
    {
        lapStartTime = Time.time;
        lapStarted = true;
        lapEnded = false;
        Debug.Log("Lap started.");
    }

    public void EndLap()
    {
        lapEndTime = Time.time;
        lapStarted = false;
        lapEnded = true;
        raceWinLose.ShowWinScreen(lapEndTime - lapStartTime); // Показываем экран победы с итоговым временем
        Debug.Log("Lap ended. Time: " + FormatTime(lapEndTime - lapStartTime));
    }

    public void TurnOffEngine()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public void TurnOnEngine()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float startVolume = engineSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            engineSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        engineSource.volume = 0f;
        engineSource.Stop(); // Останавливаем источник звука, когда громкость достигает нуля
    }

    private IEnumerator FadeInCoroutine()
    {
        float startVolume = engineSource.volume; // Начальная громкость при возобновлении игры
        float targetVolume = initialVolume; // Конечная громкость, которую вы хотите достичь
        float elapsedTime = 0f;

        if (startVolume > 0) // Если звук был выключен, установим начальную громкость в 0
        {
            startVolume = 0f;
        }

        engineSource.Play(); // Включаем источник звука

        while (elapsedTime < fadeInDuration)
        {
            engineSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        engineSource.volume = targetVolume; // Устанавливаем конечную громкость
    }

    // Обработчик нажатия кнопки ускорения
    public void Run()
    {
        isAccelerating = true;
        Debug.Log("Accelerate button pressed.");
    }

    // Обработчик отпускания кнопки ускорения
    public void Stop()
    {
        isAccelerating = false;
        Debug.Log("Accelerate button released.");
    }

    // Обновление отображения скорости
    private void UpdateSpeedIndicator()
    {
        if (speedIndicator != null)
        {
            float heightRatio = Mathf.Clamp(currentSpeed / maxSpeed, 0f, 1f);
            speedIndicator.sizeDelta = new Vector2(speedIndicator.sizeDelta.x, heightRatio * 107); // Изменение высоты в зависимости от скорости
        }
    }

    public void SetBrakeAcceleration(float value)
    {
        brakeAcceleration = value;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    // Свойства для проверки состояния круга
    public bool LapStarted
    {
        get { return lapStarted; }
    }

    public bool LapEnded
    {
        get { return lapEnded; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
    }
}
