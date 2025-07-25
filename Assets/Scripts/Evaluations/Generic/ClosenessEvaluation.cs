using UnityEngine;

/// <summary>
/// Comparison that evaluates the distance of a raster shape to a vector shape.
/// </summary>
[CreateAssetMenu(menuName = "Evaluation/Closeness")]
public class ClosenessComparison : GenericEvaluation
{
    [SerializeField] float scalingFactor;

    public override float Compare(IShape vector, PenStroke raster)
    {
        float averageDistance = 0;
        foreach (Vector2 point in raster.Points)
        {
            Vector2 closestPoint = vector.ClosestPointOnShapeTo(point);
            averageDistance += Vector2.Distance(point, closestPoint);
        }
        averageDistance /= raster.Points.Count;

        float closeness = averageDistance * scalingFactor;
        return Mathf.Clamp(closeness, 0, 1);
    }
}
