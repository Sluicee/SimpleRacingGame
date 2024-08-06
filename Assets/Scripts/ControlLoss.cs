using UnityEngine;

public class ControlLoss : MonoBehaviour
{
    [SerializeField] private string controlLossTag = "ControlLossArea"; // Тег для зоны потери контроля
    [SerializeField] private float baseSpinImpulse = 1000f; // Базовая сила импульса вращения
    [SerializeField] private float bounceForce = 20f; // Сила отскока
    [SerializeField] private float spinDamping = 2f; // Коэффициент затухания вращения
    [SerializeField] private CameraFollow cameraFollow; // Ссылка на скрипт CameraFollow

    [SerializeField] private RaceWinLose raceWinLose;

    private CarController carController;
    private Rigidbody rb;
    private PathFollower pathFollower;
    public bool HasLostControl { get; private set; } = false; // Флаг для проверки потери контроля

    private void Start()
    {
        carController = GetComponent<CarController>();
        rb = GetComponent<Rigidbody>();
        pathFollower = GetComponent<PathFollower>();

        if (carController == null)
        {
            Debug.LogError("CarController component not found on " + gameObject.name);
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on " + gameObject.name);
        }

        if (pathFollower == null)
        {
            Debug.LogError("PathFollower component not found on " + gameObject.name);
        }

        if (cameraFollow == null)
        {
            Debug.LogError("CameraFollow component not assigned in " + gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(controlLossTag) && !HasLostControl)
        {
            Debug.Log("Collision detected with control loss area: " + collision.gameObject.name);
            LoseControl(collision.contacts[0].normal);
        }
    }

    private void LoseControl(Vector3 collisionNormal)
    {
        if (carController == null || rb == null || pathFollower == null)
        {
            Debug.LogError("CarController, Rigidbody, or PathFollower not found, cannot lose control.");
            return;
        }

        carController.enabled = false; // Отключаем управление
        pathFollower.enabled = false;  // Отключаем следование путевым точкам
        HasLostControl = true; // Устанавливаем флаг потери контроля

        // Применение силы отскока
        rb.AddForce(collisionNormal * bounceForce, ForceMode.Impulse);
        Debug.Log("Applied bounce force: " + (collisionNormal * bounceForce));

        // Применение импульса вращения в зависимости от текущей скорости
        float currentSpeed = rb.velocity.magnitude;
        float adjustedSpinImpulse = baseSpinImpulse * (currentSpeed / 20f); // Масштабируем импульс в зависимости от скорости
        rb.AddTorque(Vector3.up * adjustedSpinImpulse, ForceMode.Impulse);
        Debug.Log("Applied spin impulse: " + (Vector3.up * adjustedSpinImpulse));

        // Останавливаем следование камеры
        if (cameraFollow != null)
        {
            cameraFollow.SetCollisionMode(true);
        }

        raceWinLose.ShowLoseScreen(); // Показываем экран проигрыша
    }

    private void FixedUpdate()
    {
        if (HasLostControl)
        {
            // Постепенное затухание вращения
            Vector3 angularVelocity = rb.angularVelocity;
            angularVelocity = Vector3.Lerp(angularVelocity, Vector3.zero, spinDamping * Time.deltaTime);

            rb.angularVelocity = angularVelocity;
        }
    }
}
