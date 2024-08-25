using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;
using UnityEditor.U2D.Path;

// A stroke of a raster pen.
public class PenStroke
{
    List<Vector2> points;

    public PenStroke(List<Vector2> points)
    {
        this.points = points;
    }

    // Returns this pen stroke's average distance to the given bezier curve
    public float CompareWithBezier(BezierContour contour)
    {
        // Flatten list of points in the bezier curve
        List<Vector2> bezierPoints = new List<Vector2>();
        foreach (BezierPathSegment segment in contour.Segments)
        {
            bezierPoints.Add(segment.P0);
            bezierPoints.Add(segment.P1);
            bezierPoints.Add(segment.P2);
        }
        List<float> distances = new List<float>(); // Closest distance to bezier curve, for each pen stroke point
        foreach (Vector2 point in points)
        {
            // Split bezier into cubic curves and find which mini-curve has the closest point
            float minDistance = float.MaxValue;
            for (int i = 0; i < bezierPoints.Count - 3;)
            {
                float t;
                Vector2 closestPoint = BezierUtility.ClosestPointOnCurve(point, bezierPoints[i], bezierPoints[i + 3], bezierPoints[i + 1], bezierPoints[i + 2], out t);
                float dist = Vector2.Distance(closestPoint, point);
                if (dist < minDistance)
                {
                    minDistance = dist;
                }
                i += 3;
            }
            distances.Add(minDistance);
        }
        // Average out distances
        float averageDistance = 0;
        foreach (float distance in distances)
        {
            averageDistance += distance;
        }
        averageDistance /= distances.Count;
        return averageDistance;
    }
}
