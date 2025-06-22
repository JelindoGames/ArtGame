using Unity.VectorGraphics;
using UnityEngine;

public class EllipseShape : IShape
{
    Vector2 center;
    float xSize;
    float ySize;
    float angle;

    public EllipseShape(Vector2 center, float xSize, float ySize, float angle)
    {
        this.center = center;
        this.xSize = xSize;
        this.ySize = ySize;
        this.angle = angle;
    }

    Vector2 RotatePointByAngle(Vector2 origPoint, float angleChange)
    {
        float origPointAngle = Mathf.Atan2(origPoint.y, origPoint.x);
        float newPointAngle = origPointAngle + angleChange;
        return origPoint.magnitude * new Vector2(Mathf.Cos(newPointAngle), Mathf.Sin(newPointAngle));
    }

    public BezierContour Contour()
    {
        // Use a four-piece cubic bezier curve to APPROXIMATE a circle
        float distanceToOuterControlPoints = 0.5522847498f; // For a four-piece curve
        Vector2 s1p1 = RotatePointByAngle(new Vector2(xSize, -distanceToOuterControlPoints * ySize), angle) + center;
        Vector2 s1p2 = RotatePointByAngle(new Vector2(xSize, 0), angle) + center;
        Vector2 s1p3 = RotatePointByAngle(new Vector2(xSize, distanceToOuterControlPoints * ySize), angle) + center;
        Vector2 s2p1 = RotatePointByAngle(new Vector2(distanceToOuterControlPoints * xSize, ySize), angle) + center;
        Vector2 s2p2 = RotatePointByAngle(new Vector2(0, ySize), angle) + center;
        Vector2 s2p3 = RotatePointByAngle(new Vector2(-distanceToOuterControlPoints * xSize, ySize), angle) + center;
        Vector2 s3p1 = RotatePointByAngle(new Vector2(-xSize, distanceToOuterControlPoints * ySize), angle) + center;
        Vector2 s3p2 = RotatePointByAngle(new Vector2(-xSize, 0), angle) + center;
        Vector2 s3p3 = RotatePointByAngle(new Vector2(-xSize, -distanceToOuterControlPoints * ySize), angle) + center;
        Vector2 s4p1 = RotatePointByAngle(new Vector2(-distanceToOuterControlPoints * xSize, -ySize), angle) + center;
        Vector2 s4p2 = RotatePointByAngle(new Vector2(0, -ySize), angle) + center;
        Vector2 s4p3 = RotatePointByAngle(new Vector2(distanceToOuterControlPoints * xSize, -ySize), angle) + center;

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
}
