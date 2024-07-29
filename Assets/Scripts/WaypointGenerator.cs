using UnityEngine;

public class WaypointGenerator : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform waypointPrefab;
    public Transform car; // Ссылка на объект машины
    public Transform[] waypoints;

    void Start()
    {
        GenerateWaypoints();
    }

    void GenerateWaypoints()
    {
        int numPoints = lineRenderer.positionCount;
        waypoints = new Transform[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            Vector3 pointPosition = lineRenderer.GetPosition(i);
            Transform waypoint = Instantiate(waypointPrefab, pointPosition, Quaternion.identity);
            waypoint.name = "Waypoint" + i;
            waypoint.SetParent(transform);
            waypoints[i] = waypoint;
        }

        if (car != null)
        {
            PathFollower pathFollower = car.GetComponent<PathFollower>();
            if (pathFollower != null)
            {
                pathFollower.waypoints = waypoints;
            }
        }
        else
        {
            Debug.LogError("Car object is not assigned in WaypointGenerator");
        }
    }
}
