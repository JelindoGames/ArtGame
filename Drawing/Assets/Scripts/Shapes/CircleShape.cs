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

    public float CompareToStroke(PenStroke stroke)
    {
        throw new System.NotImplementedException();
    }
}
