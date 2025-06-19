using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

// A stroke of a raster pen.
public class PenStroke
{
    List<Vector2> points;

    public PenStroke(List<Vector2> points)
    {
        this.points = points;
    }

    // Returns this pen stroke's average distance to the given bezier curve
    public string CompareWithBezier(BezierContour contour)
    {
        // Flatten list of key points (curve-defining points) in the bezier curve
        List<Vector2> keyBezierPoints = new List<Vector2>();
        foreach (BezierPathSegment segment in contour.Segments)
        {
            keyBezierPoints.Add(segment.P0);
            keyBezierPoints.Add(segment.P1);
            keyBezierPoints.Add(segment.P2);
        }

        // Get representative bezier points (points on the curve which don't necessarily define it)
        List<Vector2> bezierPoints = new();
        for (int i = 0; i < keyBezierPoints.Count - 3; i += 3)
        {
            bezierPoints.AddRange(BezierUtils.GetRepresentativePoints(keyBezierPoints[i], keyBezierPoints[i + 3], keyBezierPoints[i + 1], keyBezierPoints[i + 2], 20));
        }

        // For each representative bezier point, find the distance to the CLOSEST stroke point
        List<float> distances = new();
        foreach (Vector2 bezierPoint in bezierPoints)
        {
            float distance = float.MaxValue;
            foreach (Vector2 strokePoint in points)
            {
                float newDistance = Vector2.Distance(strokePoint, bezierPoint);
                if (newDistance < distance)
                {
                    distance = newDistance;
                }
            }
            distances.Add(distance);
        }

        Debug.Log("AMOUNT: " + points.Count);

        // Average out distances
        float averageDistance = 0;
        foreach (float distance in distances)
        {
            averageDistance += distance;
        }
        averageDistance /= distances.Count;

        // Find standard deviation
        float standardDeviation = 0;
        foreach (float distance in distances)
        {
            standardDeviation += Mathf.Pow(distance - averageDistance, 2);
        }
        standardDeviation /= distances.Count;
        standardDeviation = Mathf.Sqrt(standardDeviation);

        return "AVG: " + averageDistance + "\n" +
            "STD DEV: " + standardDeviation;
    }
}
