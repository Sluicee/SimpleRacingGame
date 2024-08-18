using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public Transform[] wheels; // Массив всех колес
    public float wheelRadius = 0.5f; // Радиус колеса

    private CarController carController;

    private void Start()
    {
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        RotateWheels();
    }

    private void RotateWheels()
    {
        // Текущая скорость машины
        float speed = carController.currentSpeed;

        // Расчет угла вращения колеса
        float rotationAmount = speed / (2 * Mathf.PI * wheelRadius) * 360 * Time.deltaTime;

        // Вращаем каждое колесо вокруг его локальной оси
        foreach (Transform wheel in wheels)
        {
            // Ось вращения Y или Z может зависеть от вашей модели (обычно это X, но может быть и Y или Z).
            wheel.Rotate(Vector3.up * rotationAmount, Space.Self);
        }
    }
}
