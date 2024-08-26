using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private WaypointGenerator waypointGenerator;
    [SerializeField] private float reachThreshold = 0.1f; // Радиус, в котором точка считается достигнутой
    [SerializeField] private float rotationSpeed = 5f; // Скорость поворота
    [SerializeField] private float wheelTurnSpeed = 2f; // Скорость поворота колес
    [SerializeField] private Transform frontLeftWheel; // Переднее левое колесо
    [SerializeField] private Transform frontRightWheel; // Переднее правое колесо
    [SerializeField] private float maxWheelAngle = 30f; // Максимальный угол поворота колес
    [SerializeField] private float predictionDistance = 3f; // Расстояние для предсказания точки поворота
    [SerializeField] private ControlLoss controlLoss; // Ссылка на скрипт ControlLoss

    private List<Vector3> smoothedWaypoints;
    private int currentWaypointIndex = 0;

    public void Initialize(WaypointGenerator _waypointGenerator)
    {
        waypointGenerator = _waypointGenerator;
    }

    private void Start()
    {
        controlLoss = GetComponent<ControlLoss>();
        if (waypointGenerator != null)
        {
            smoothedWaypoints = waypointGenerator.GetSmoothedWaypoints();
            if (smoothedWaypoints == null || smoothedWaypoints.Count == 0)
            {
                Debug.LogError("Smoothed waypoints not generated or empty");
            }
        }
        else
        {
            Debug.LogError("WaypointGenerator not assigned");
        }
    }

    private void Update()
    {
        if (controlLoss != null && controlLoss.HasLostControl)
        {
            // Не следуем за путевыми точками, если управление потеряно
            return;
        }

        if (smoothedWaypoints != null && currentWaypointIndex < smoothedWaypoints.Count)
        {
            Vector3 targetWaypoint = smoothedWaypoints[currentWaypointIndex];
            Vector3 direction = targetWaypoint - transform.position;

            // Предсказание точки для поворота колес
            Vector3 targetWheelWaypoint = targetWaypoint + direction.normalized * predictionDistance;
            Vector3 wheelDirection = targetWheelWaypoint - transform.position;

            // Проверка достижения путевой точки
            if (Vector3.Distance(transform.position, targetWaypoint) < reachThreshold)
            {
                currentWaypointIndex++;
            }

            // Поворот машины в направлении движения
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            // Проверка наличия передних колес
            if (frontLeftWheel == null || frontRightWheel == null)
            {
                return;
            }

            // Рассчитываем угол поворота передних колес
            float angle = Vector3.SignedAngle(transform.forward, wheelDirection, Vector3.up);
            float targetWheelAngle = Mathf.Clamp(angle, -maxWheelAngle, maxWheelAngle);

            // Используем Mathf.Lerp для плавного изменения угла
            float wheelAngle = Mathf.LerpAngle(0, targetWheelAngle, wheelTurnSpeed * Time.deltaTime);

            // Применяем угол поворота к передним колесам с учетом их начальной ориентации
            frontLeftWheel.localRotation = Quaternion.Euler(-90, wheelAngle - 90, 0);
            frontRightWheel.localRotation = Quaternion.Euler(-90, wheelAngle - 90, 0);
        }
    }
}
