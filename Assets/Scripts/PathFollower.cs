using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private WaypointGenerator waypointGenerator;
    [SerializeField] private float reachThreshold = 0.1f; // Радиус, в котором точка считается достигнутой
    [SerializeField] private float rotationSpeed = 5f; // Скорость поворота
    [SerializeField] private ControlLoss controlLoss; // Ссылка на скрипт ControlLoss

    private List<Vector3> smoothedWaypoints;
    private int currentWaypointIndex = 0;

    private void Start()
    {
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
        }
    }
}
