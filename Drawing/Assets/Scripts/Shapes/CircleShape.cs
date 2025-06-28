using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class CircleShape : IShape
{
    Vector2 center;
    float radius;

    public CircleShape(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    // Override default IShape implementation for efficiency
    public Vector2 ClosestPointOnShapeTo(Vector2 point)
    {
        Vector2 centerToPointVector = point - center;
        float centerToPointAngle = Mathf.Atan2(centerToPointVector.y, centerToPointVector.x);
        float x = center.x + (Mathf.Cos(centerToPointAngle) * radius);
        float y = center.y + (Mathf.Sin(centerToPointAngle) * radius);
        return new Vector2(x, y);
    }

    public BezierContour Contour()
    {
        // Use a four-piece cubic bezier curve to APPROXIMATE a circle
        float distanceToOuterControlPoints = 0.5522847498f; // For a four-piece curve
        Vector2 s1p1 = center + new Vector2(1, -distanceToOuterControlPoints) * radius;
        Vector2 s1p2 = center + new Vector2(1, 0) * radius;
        Vector2 s1p3 = center + new Vector2(1, distanceToOuterControlPoints) * radius;
        Vector2 s2p1 = center + new Vector2(distanceToOuterControlPoints, 1) * radius;
        Vector2 s2p2 = center + new Vector2(0, 1) * radius;
        Vector2 s2p3 = center + new Vector2(-distanceToOuterControlPoints, 1) * radius;
        Vector2 s3p1 = center + new Vector2(-1, distanceToOuterControlPoints) * radius;
        Vector2 s3p2 = center + new Vector2(-1, 0) * radius;
        Vector2 s3p3 = center + new Vector2(-1, -distanceToOuterControlPoints) * radius;
        Vector2 s4p1 = center + new Vector2(-distanceToOuterControlPoints, -1) * radius;
        Vector2 s4p2 = center + new Vector2(0, -1) * radius;
        Vector2 s4p3 = center + new Vector2(distanceToOuterControlPoints, -1) * radius;

        var segments = new BezierPathSegment[]
        {
            new BezierPathSegment() { P0 = s1p2, P1 = s1p3, P2 = s2p1 },
            new BezierPathSegment() { P0 = s2p2, P1 = s2p3, P2 = s3p1 },
            new BezierPathSegment() { P0 = s3p2, P1 = s3p3, P2 = s4p1 },
            new BezierPathSegment() { P0 = s4p2, P1 = s4p3, P2 = s1p1 },
            new BezierPathSegment() { P0 = s1p2 }
        };
        return new BezierContour()
        {
            Segments = segments,
            Closed = true
        };
    }

    // Override default IShape implementation for details
    public string CompareToStroke(PenStroke stroke)
    {
        List<Vector2> points = stroke.Points;
        List<float> distances = new();
        float averageDistance = 0;
        foreach (Vector2 point in points)
        {
            Vector2 closestPointOnLine = ClosestPointOnShapeTo(point);
            float distance = Vector2.Distance(point, closestPointOnLine);
            distances.Add(distance);
            averageDistance += distance;
        }
        averageDistance /= points.Count;

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

        float incompleteScore = 0; // The less of the circle the stroke covers, the higher the incomplete score is
        for (float theta = 0; theta < Mathf.PI * 2 - Mathf.Epsilon; theta += Mathf.PI / 4)
        {
            Vector2 pointAtAngle = new(center.x + (Mathf.Cos(theta) * radius), center.y + (Mathf.Sin(theta) * radius));
            Vector2 closestPointOnStroke = stroke.ClosestPointOnStrokeTo(pointAtAngle);
            float distance = Vector2.Distance(closestPointOnStroke, pointAtAngle);
            incompleteScore += distance;
        }
        incompleteScore /= 8; // Average out the points we looked at

        return $"AVG: {averageDistance}\nSHAKINESS: {shakiness}\nINCOMPLETENESS: {incompleteScore}";
    }
}
