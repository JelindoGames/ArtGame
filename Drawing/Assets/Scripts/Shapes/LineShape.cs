using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

/// <summary>
/// A vector graphics line segment that can be drawn or compared to a raster pen stroke.
/// </summary>
public class LineShape : IShape
{
    public Vector2 StartingPoint { get; private set; }
    public Vector2 EndingPoint { get; private set; }

    public LineShape(Vector2 startingPoint, Vector2 endingPoint)
    {
        this.StartingPoint = startingPoint;
        this.EndingPoint = endingPoint;
    }

    /// <summary>
    /// Returns t, the point on the line where 0 is startingPoint and 1 is endingPoint,
    /// which is closet to the given point. The returned number can be less than 0
    /// or greater than 1.
    /// </summary>
    float ClosestPointOnShapeUnbounded(Vector2 point)
    {
        Vector2 lineVector = EndingPoint - StartingPoint;
        if (lineVector.x == 0) // Vertical line case
        {
            return (point.y - StartingPoint.y) / (EndingPoint.y - StartingPoint.y);
        }
        if (lineVector.y == 0) // Horizontal line case
        {
            return (point.x - StartingPoint.x) / (EndingPoint.x - StartingPoint.x);
        }

        float t; // The spot on this line (where 0 = startingPoint to 1 = endingPoint) which is closest
        // Below is based on algebra of the intersection point of two lines
        t = point.x + (((point.y * lineVector.y) - (StartingPoint.y * lineVector.y)) / lineVector.x) - StartingPoint.x;
        t /= lineVector.x + (lineVector.y * lineVector.y / lineVector.x);
        return t;
    }

    // Override default IShape implementation for efficiency
    public Vector2 ClosestPointOnShapeTo(Vector2 point)
    {
        float t = Mathf.Clamp(ClosestPointOnShapeUnbounded(point), 0, 1);
        return Vector3.Lerp(StartingPoint, EndingPoint, t);
    }

    public BezierContour Contour()
    {
        Vector2 midPoint = (StartingPoint + EndingPoint) / 2;
        var segments = new BezierPathSegment[]
        {
            new BezierPathSegment() { P0 = StartingPoint, P1 = midPoint, P2 = EndingPoint },
            new BezierPathSegment() { P0 = EndingPoint }
        };
        return new BezierContour()
        {
            Segments = segments,
            Closed = false
        };
    }

    public List<Vector2> DefiningPoints()
    {
        return new List<Vector2>() { StartingPoint, EndingPoint };
    }
}
