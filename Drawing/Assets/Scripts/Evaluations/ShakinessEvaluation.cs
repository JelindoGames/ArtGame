using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Comparison that evaluates the shakiness of a raster shape.
/// </summary>
[CreateAssetMenu(menuName = "Evaluation/Shakiness")]
public class ShakinessEvaluation : GenericEvaluation
{
    [SerializeField] float scalingFactor;

    public override float Compare(IShape vector, PenStroke raster)
    {
        List<Vector2> points = raster.Points;
        List<float> distances = new();
        foreach (Vector2 point in points)
        {
            Vector2 closestPointOnLine = vector.ClosestPointOnShapeTo(point);
            float distance = Vector2.Distance(point, closestPointOnLine);
            distances.Add(distance);
        }

        // Shakiness is defined by how much the distance varies as the stroke goes on
        float shakiness = 0;
        if (points.Count != 0)
        {
            for (int i = 1; i < 10; i++)
            {
                int a = distances.Count * (i - 1) / 10;
                int b = distances.Count * i / 10;
                shakiness += Mathf.Abs(distances[b] - distances[a]);
            }
            shakiness /= 10;
        }

        shakiness *= scalingFactor;
        return Mathf.Clamp(shakiness, 0, 1);
    }
}
