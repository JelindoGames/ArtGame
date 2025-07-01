using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Comparison that evaluates the shakiness of a raster shape.
/// (Shakiness is defined by how much the distance varies as the stroke goes on.)
/// </summary>
[CreateAssetMenu(menuName = "Evaluation/Shakiness")]
public class ShakinessEvaluation : GenericEvaluation
{
    [SerializeField] float scalingFactor;
    [SerializeField] float pointsToSample;

    public override float Compare(IShape vector, PenStroke raster)
    {
        if (raster.Points.Count == 0)
        {
            return 0;
        }

        List<int> indexesToSample = new();
        for (int i = 0; i < pointsToSample; i++)
        {
            int idx = Random.Range(0, raster.Points.Count);
            indexesToSample.Add(idx);
        }
        indexesToSample.Sort();

        // Take the standard deviation of the CHANGES in distance over time
        float shakiness = 0;
        for (int i = 1; i < indexesToSample.Count; i++)
        {
            Vector2 pointA = raster.Points[indexesToSample[i - 1]];
            Vector2 pointB = raster.Points[indexesToSample[i]];
            float distanceA = Vector2.Distance(pointA, vector.ClosestPointOnShapeTo(pointA));
            float distanceB = Vector2.Distance(pointB, vector.ClosestPointOnShapeTo(pointB));
            shakiness += Mathf.Pow(distanceB - distanceA, 2);
        }
        shakiness /= indexesToSample.Count - 1;
        shakiness = Mathf.Sqrt(shakiness);

        shakiness *= scalingFactor;
        return Mathf.Clamp(shakiness, 0, 1);
    }
}
