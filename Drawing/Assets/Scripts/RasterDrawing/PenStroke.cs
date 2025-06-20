using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

// A stroke of a raster pen.
public class PenStroke
{
    public List<Vector2> Points { get; private set; }

    public PenStroke(List<Vector2> points)
    {
        Points = points;
    }

    public string CompareWithShape(IShape shape)
    {
        Debug.Log(Points.Count);
        return "Similarity: " + shape.CompareToStroke(this);
    }

    // Returns this pen stroke's average distance to the given bezier curve
    public string CompareWithBezier(BezierContour contour)
    {
        /*
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
        List<float> bezierToStrokeDistances = new();
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
            bezierToStrokeDistances.Add(distance);
        }

        // For each stroke point, find the closest representative bezier point
        List<float> strokeToBezierDistances = new();
        foreach (Vector2 strokePoint in points)
        {
            float distance = float.MaxValue;
            foreach (Vector2 bezierPoint in bezierPoints)
            {
                float newDistance = Vector2.Distance(strokePoint, bezierPoint);
                if (newDistance < distance)
                {
                    distance = newDistance;
                }
            }
            strokeToBezierDistances.Add(distance);
        }

        Debug.Log("AMOUNT: " + points.Count);

        // Find average of bezier to stroke
        float averageBezierToStrokeDistance = 0;
        foreach (float distance in bezierToStrokeDistances)
        {
            averageBezierToStrokeDistance += distance;
        }
        averageBezierToStrokeDistance /= bezierToStrokeDistances.Count;

        // Find average of stroke to bezier
        float averageStrokeToBezierDistance = 0;
        foreach (float distance in strokeToBezierDistances)
        {
            averageStrokeToBezierDistance += distance;
        }
        averageStrokeToBezierDistance /= strokeToBezierDistances.Count;

        // Find 10 distances and find how much they vary
        float standardDeviation = 0;
        for (int i = 1; i < 10; i++)
        {
            int a = strokeToBezierDistances.Count * (i - 1) / 10;
            int b = strokeToBezierDistances.Count * i / 10;
            standardDeviation += Mathf.Abs(strokeToBezierDistances[b] - strokeToBezierDistances[a]);
        }
        standardDeviation /= strokeToBezierDistances.Count;
        /*
        foreach (float distance in strokeToBezierDistances)
        {
            standardDeviation += Mathf.Pow(distance - averageStrokeToBezierDistance, 2);
        }
        */

        /*
        return "AVG B2S: " + averageBezierToStrokeDistance + "\n" +
            "AVG S2B: " + averageStrokeToBezierDistance + "\n" +
            "STD DEV S2B: " + standardDeviation;
        */
        return "Deprecated";
    }
}
