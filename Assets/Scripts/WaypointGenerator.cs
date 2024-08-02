using System.Collections.Generic;
using UnityEngine;

public class WaypointGenerator : MonoBehaviour
{
    public int pointsPerSegment = 10; // Количество точек на сегмент сплайна

    private List<Vector3> smoothedWaypoints;
    private LineRenderer lineRenderer;
    private Transform[] waypoints;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found.");
            return;
        }

        InitializeWaypointsFromLineRenderer();
        GenerateSmoothPath();
    }

    void InitializeWaypointsFromLineRenderer()
    {
        int lineRendererPointCount = lineRenderer.positionCount;
        waypoints = new Transform[lineRendererPointCount];

        for (int i = 0; i < lineRendererPointCount; i++)
        {
            Vector3 pointPosition = lineRenderer.GetPosition(i);
            GameObject waypointObject = new GameObject("Waypoint" + i);
            waypointObject.transform.position = pointPosition;
            waypointObject.transform.parent = transform; // To keep the hierarchy organized
            waypoints[i] = waypointObject.transform;
        }
    }

    void GenerateSmoothPath()
    {
        smoothedWaypoints = new List<Vector3>();

        if (waypoints.Length < 4)
        {
            Debug.LogError("Not enough waypoints to generate a smooth path");
            return;
        }

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Vector3 p0 = waypoints[Mathf.Clamp(i - 1, 0, waypoints.Length - 1)].position;
            Vector3 p1 = waypoints[i].position;
            Vector3 p2 = waypoints[Mathf.Clamp(i + 1, 0, waypoints.Length - 1)].position;
            Vector3 p3 = waypoints[Mathf.Clamp(i + 2, 0, waypoints.Length - 1)].position;

            for (int j = 0; j < pointsPerSegment; j++)
            {
                float t = j / (float)pointsPerSegment;
                Vector3 position = SplineUtils.GetCatmullRomPosition(t, p0, p1, p2, p3);
                smoothedWaypoints.Add(position);
            }
        }

        // Add the last waypoint
        smoothedWaypoints.Add(waypoints[waypoints.Length - 1].position);

        // Optionally, visualize the smoothed path using LineRenderer
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = smoothedWaypoints.Count;
            lineRenderer.SetPositions(smoothedWaypoints.ToArray());
        }

        // Отладочное сообщение для проверки генерации путевых точек
        Debug.Log("Smoothed waypoints generated: " + smoothedWaypoints.Count);
    }

    public List<Vector3> GetSmoothedWaypoints()
    {
        return smoothedWaypoints;
    }
}
