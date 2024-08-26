using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    private CarController carController;
    [SerializeField] private AudioSource engineSource;
    [SerializeField] private float minPitch = 0.5f;   // Минимальный питч при нулевой скорости
    [SerializeField] private float maxPitch = 2.0f;   // Максимальный питч при максимальной скорости
    [SerializeField] private float maxSpeed = 100f;   // Максимальная скорость, при которой будет достигаться maxPitch

    private void Start()
    {
        carController = GetComponent<CarController>();
        if (engineSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
        }

        maxSpeed = carController.MaxSpeed * 2;
    }

    private void Update()
    {
        float speed = carController.currentSpeed;
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed); // Нормализуем скорость от 0 до 1
        engineSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);
    }
}
