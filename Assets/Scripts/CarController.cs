using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 5f;       // Ускорение машины
    public float maxSpeed = 20f;          // Максимальная скорость машины
    public float brakeAcceleration = 10f; // Тормозное ускорение
    public float boostMultiplier = 2f;    // Множитель ускорения при бусте
    public float boostDuration = 5f;      // Длительность буста

    private float currentSpeed = 0f;      // Текущая скорость машины
    private bool isBoosting = false;      // Флаг, указывающий на активацию буста
    private float boostEndTime = 0f;      // Время окончания буста

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Управление ускорением
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
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
        if (Input.GetKeyDown(KeyCode.Space) && !isBoosting)
        {
            StartBoost();
        }

        if (isBoosting && Time.time > boostEndTime)
        {
            EndBoost();
        }
    }

    void StartBoost()
    {
        isBoosting = true;
        currentSpeed *= boostMultiplier;
        boostEndTime = Time.time + boostDuration;
    }

    void EndBoost()
    {
        isBoosting = false;
        currentSpeed /= boostMultiplier;
    }
}
