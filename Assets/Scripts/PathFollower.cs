using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public WaypointGenerator waypointGenerator;
    public float reachThreshold = 0.1f; // Радиус, в котором точка считается достигнутой
    public float rotationSpeed = 5f; // Скорость поворота

    private List<Vector3> smoothedWaypoints;
    private int currentWaypointIndex = 0;

    void Start()
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

    void Update()
    {
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
