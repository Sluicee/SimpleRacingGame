using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] private float acceleration = 5f;       // Ускорение машины
    [SerializeField] private float maxSpeed = 20f;          // Максимальная скорость машины
    [SerializeField] private float brakeAcceleration = 10f; // Тормозное ускорение
    [SerializeField] private float boostMultiplier = 2f;    // Множитель ускорения при бусте
    [SerializeField] private float boostDuration = 5f;      // Длительность буста

    [SerializeField] private TextMeshProUGUI speedText;     // Ссылка на TextMeshPro текст для отображения скорости
    [SerializeField] private TextMeshProUGUI lapTimeText;   // Ссылка на TextMeshPro текст для отображения времени круга
    [SerializeField] private RectTransform speedIndicator;  // Ссылка на RectTransform для отображения текущей скорости

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

    private bool isAccelerating = false;  // Флаг, указывающий, что кнопка ускорения зажата

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentAcceleration = acceleration; // Устанавливаем текущее ускорение в начальное значение
        lapTimeText.text = "Lap Time: 00:00.0"; // Инициализируем текст времени круга

        // Установка начального значения для speedIndicator
        if (speedIndicator != null)
        {
            speedIndicator.pivot = new Vector2(0f, 0f); // Установка якоря снизу
            speedIndicator.sizeDelta = new Vector2(speedIndicator.sizeDelta.x, 0f); // Начальная высота = 0
        }
    }

    void Update()
    {
        // Управление ускорением
        if (isAccelerating)
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
        if (Input.GetKeyDown(KeyCode.Space) && !isBoosting && !hasBoosted)
        {
            StartBoost();
        }

        if (isBoosting && Time.time > boostEndTime)
        {
            EndBoost();
        }

        // Обновление TextMeshPro текста с текущей скоростью
        if (speedText != null)
        {
            speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed * 10).ToString() + " km/h"; // Умножаем на 10 для более реалистичных значений скорости
        }

        // Обновление времени круга
        if (lapStarted && !lapEnded)
        {
            float lapTime = Time.time - lapStartTime;
            lapTimeText.text = "Lap Time: " + FormatTime(lapTime);
        }

        // Обновление отображения скорости
        UpdateSpeedIndicator();
    }

    public void StartBoost()
    {
        isBoosting = true;
        currentAcceleration = acceleration * boostMultiplier; // Увеличиваем ускорение
        boostEndTime = Time.time + boostDuration;
        hasBoosted = true; // Устанавливаем флаг использования буста
        Debug.Log("Boost activated.");
    }

    private void EndBoost()
    {
        isBoosting = false;
        currentAcceleration = acceleration; // Возвращаемся к нормальному ускорению
        Debug.Log("Boost ended.");
    }

    // Форматирование времени в минуты, секунды и миллисекунды
    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time - Mathf.Floor(time)) * 1000);

        return string.Format("{0:D2}:{1:D2}.{2:D3}", minutes, seconds, milliseconds);
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
        Debug.Log("Lap ended. Time: " + FormatTime(lapEndTime - lapStartTime));
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
            speedIndicator.sizeDelta = new Vector2(speedIndicator.sizeDelta.x, heightRatio * 55); // Изменение высоты в зависимости от скорости
        }
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
}
