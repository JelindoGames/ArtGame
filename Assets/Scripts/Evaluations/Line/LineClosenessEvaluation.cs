using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Comparison that evaluates the distance of a raster line to a vector line.
/// Takes advantage of specific line attributes to make a more detailed comparison.
/// </summary>
[CreateAssetMenu(menuName = "Evaluation/Line/Line Closeness")]
public class LineClosenessEvaluation : LineEvaluation
{
    [SerializeField] float scalingFactor;

    public override float Compare(LineShape vector, PenStroke raster)
    {
        List<Vector2> points = raster.Points;
        List<float> distances = new();
        float averageDistance = 0;
        foreach (Vector2 point in points)
        {
            Vector2 closestPointOnLine = vector.ClosestPointOnShapeTo(point);
            float distance = Vector2.Distance(point, closestPointOnLine);
            distances.Add(distance);
            averageDistance += distance;
        }
        averageDistance /= points.Count;

        Vector2 closestToStartingPoint = raster.ClosestPointOnStrokeTo(vector.StartingPoint);
        Vector2 closestToEndingPoint = raster.ClosestPointOnStrokeTo(vector.EndingPoint);
        float closestDistanceToStart = Vector2.Distance(closestToStartingPoint, vector.StartingPoint);
        float closestDistanceToEnd = Vector2.Distance(closestToEndingPoint, vector.EndingPoint);

        float closeness = (averageDistance + closestDistanceToStart + closestDistanceToEnd) * scalingFactor;
        return Mathf.Clamp(closeness, 0, 1);
    }
}
