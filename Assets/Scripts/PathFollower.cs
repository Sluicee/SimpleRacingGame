using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] waypoints; // Массив путевых точек
    public float reachThreshold = 0.1f; // Радиус, в котором точка считается достигнутой
    public float turnSpeed = 2f; // Скорость поворота машины

    private int currentWaypointIndex = 0; // Индекс текущей путевой точки
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = targetWaypoint.position - transform.position;

            // Проверка достижения путевой точки
            if (Vector3.Distance(transform.position, targetWaypoint.position) < reachThreshold)
            {
                currentWaypointIndex++;
            }

            // Поворот машины в направлении движения
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

                // Поворот через Rigidbody
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.deltaTime));
            }
        }
    }
}
