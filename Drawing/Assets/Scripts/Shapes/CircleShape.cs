using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class CircleShape : IShape
{
    public Vector2 Center { get; private set; }
    public float Radius { get; private set; }

    public CircleShape(Vector2 center, float radius)
    {
        this.Center = center;
        this.Radius = radius;
    }

    // Override default IShape implementation for efficiency
    public Vector2 ClosestPointOnShapeTo(Vector2 point)
    {
        Vector2 centerToPointVector = point - Center;
        float centerToPointAngle = Mathf.Atan2(centerToPointVector.y, centerToPointVector.x);
        float x = Center.x + (Mathf.Cos(centerToPointAngle) * Radius);
        float y = Center.y + (Mathf.Sin(centerToPointAngle) * Radius);
        return new Vector2(x, y);
    }

    public BezierContour Contour()
    {
        // Use a four-piece cubic bezier curve to APPROXIMATE a circle
        float distanceToOuterControlPoints = 0.5522847498f; // For a four-piece curve
        Vector2 s1p1 = Center + new Vector2(1, -distanceToOuterControlPoints) * Radius;
        Vector2 s1p2 = Center + new Vector2(1, 0) * Radius;
        Vector2 s1p3 = Center + new Vector2(1, distanceToOuterControlPoints) * Radius;
        Vector2 s2p1 = Center + new Vector2(distanceToOuterControlPoints, 1) * Radius;
        Vector2 s2p2 = Center + new Vector2(0, 1) * Radius;
        Vector2 s2p3 = Center + new Vector2(-distanceToOuterControlPoints, 1) * Radius;
        Vector2 s3p1 = Center + new Vector2(-1, distanceToOuterControlPoints) * Radius;
        Vector2 s3p2 = Center + new Vector2(-1, 0) * Radius;
        Vector2 s3p3 = Center + new Vector2(-1, -distanceToOuterControlPoints) * Radius;
        Vector2 s4p1 = Center + new Vector2(-distanceToOuterControlPoints, -1) * Radius;
        Vector2 s4p2 = Center + new Vector2(0, -1) * Radius;
        Vector2 s4p3 = Center + new Vector2(distanceToOuterControlPoints, -1) * Radius;

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

    public List<Vector2> DefiningPoints()
    {
        return new List<Vector2>()
        {
            Center + (Radius * Vector2.right),
            Center + (Radius * Vector2.up),
            Center + (Radius * Vector2.down),
            Center + (Radius * Vector2.left),
        };
    }
}
