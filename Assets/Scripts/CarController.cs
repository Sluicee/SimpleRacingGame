using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    [SerializeField] private float acceleration = 5f;       // Ускорение машины
    [SerializeField] private float maxSpeed = 20f;          // Максимальная скорость машины
    [SerializeField] private float brakeAcceleration = 10f; // Тормозное ускорение
    [SerializeField] private float boostMultiplier = 2f;    // Множитель ускорения при бусте
    [SerializeField] private float boostDuration = 5f;      // Длительность буста

    public float currentSpeed = 0f;       // Текущая скорость машины
    private bool isBoosting = false;      // Флаг активации буста
    private bool hasBoosted = false;      // Флаг, указывающий, что буст уже был использован
    private float boostEndTime = 0f;      // Время окончания буста

    private float currentAcceleration;    // Текущее ускорение машины
    private Rigidbody rb;
    [SerializeField] private TextMeshProUGUI speedText;     // Ссылка на TextMeshPro текст для отображения скорости

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentAcceleration = acceleration; // Устанавливаем текущее ускорение в начальное значение
    }

    void Update()
    {
        // Управление ускорением
        if (Input.GetKey(KeyCode.W))
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
    }

    private void StartBoost()
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
}
