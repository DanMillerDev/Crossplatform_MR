using System.Collections.Generic;
using UnityEngine;

public class PoissonDiscSampling : MonoBehaviour
{
    List<Vector2> m_PoissonPointsList = new List<Vector2>();

    Vector3 m_PlaneNormal;
    
    [SerializeField]
    float m_MinDistance = 0.1f;

    public float MinDistance
    {
        get { return m_MinDistance; }
    }
    GameObject m_HitPlane;

    const int k_SampleCount = 30;

    public List<Vector2> GetPointsRelativeToPlane(float planeSizeX, float planeSizeY)
    {
        var cellSize = m_MinDistance / Mathf.Sqrt(2);
        var grid = new int[Mathf.CeilToInt(planeSizeX / cellSize), Mathf.CeilToInt(planeSizeY / cellSize)];
        
        var points = new List<Vector2>();
        var processingPoints = new List<Vector2>();
        var gridRect = new Rect(0, 0, planeSizeX, planeSizeY);
        var firstPoint = new Vector2(Random.Range(0, planeSizeX), Random.Range(0, planeSizeY));

        processingPoints.Add(firstPoint);

        while (processingPoints.Count > 0)
        {
            var index = Random.Range(0, processingPoints.Count);
            var point = processingPoints[index];
            bool validPointFound = false;

            for (int i = 0; i < k_SampleCount; i++)
            {
                var newPoint = GenerateRandomPointAround(point);

                if (gridRect.Contains(newPoint) && IsPointFarEnoughAway(grid, newPoint, cellSize, points))
                {
                    points.Add(newPoint);
                    processingPoints.Add(newPoint);
                    grid[(int)(newPoint.x / cellSize), (int)(newPoint.y / cellSize)] = points.Count;
                    validPointFound = true;
                    break;
                }
            }

            if (!validPointFound)
            {
                processingPoints.RemoveAt(index);
            }
        }

        m_PoissonPointsList = points;
        return points;
    }

    Vector2 GetCell(Vector2 point, float cellSize)
    {
        var x = (int)(point.x / cellSize);
        var y = (int)(point.y / cellSize);
        return new Vector2(x, y);
    }

    Vector2 GenerateRandomPointAround(Vector2 point)
    {
        var r1 = Random.value;
        var r2 = Random.value;

        var radius = m_MinDistance * (r1 + 1);
        var angle = 2 * Mathf.PI * r2;
        var newX = point.x + radius * Mathf.Cos(angle);
        var newY = point.y + radius * Mathf.Sin(angle);
        return new Vector2(newX, newY);
    }

    bool IsPointFarEnoughAway(int[,] grid, Vector2 point, float cellSize, List<Vector2> pointsList)
    {
        var gridPoint = GetCell(point, cellSize);
        
        var startX = (int)Mathf.Max(0, gridPoint.x - 2);
        var endX = (int)Mathf.Min(gridPoint.x + 2, grid.GetLength(0) - 1);
        var startY = (int)Mathf.Max(0, gridPoint.y - 2);
        var endY = (int)Mathf.Min(gridPoint.y + 2, grid.GetLength(1) - 1);

        for (int i = startX; i <= endX; i++)
        {
            for (int j = startY; j <= endY; j++)
            {
                var pointIndex = grid[i,j] - 1;
                if (pointIndex != -1)
                {
                    if (Vector2.Distance(pointsList[pointIndex], point) < m_MinDistance)
                    {
                        return false;
                    }
                }
            }
        } 

        return true;
    }
}
