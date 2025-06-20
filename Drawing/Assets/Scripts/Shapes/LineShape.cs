using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

/// <summary>
/// A vector graphics line segment that can be drawn or compared to a raster pen stroke.
/// </summary>
public class LineShape : IShape
{
    Vector2 startingPoint;
    Vector2 endingPoint;

    public LineShape(Vector2 startingPoint, Vector2 endingPoint)
    {
        this.startingPoint = startingPoint;
        this.endingPoint = endingPoint;
    }

    /// <summary>
    /// Finds the point on this line that the given point is closest to.
    /// </summary>
    Vector2 ClosestPointOnLineTo(Vector2 point)
    {
        Vector2 lineVector = endingPoint - startingPoint;
        if (lineVector.x == 0) // Vertical line case
        {
            return new Vector2(startingPoint.x, Mathf.Clamp(point.y, startingPoint.y, endingPoint.y));
        }
        if (lineVector.y == 0) // Horizontal line case
        {
            return new Vector2(Mathf.Clamp(point.x, startingPoint.x, endingPoint.x), startingPoint.y);
        }

        float t; // The spot on this line segment (from 0 = startingPoint to 1 = endingPoint) which is closest
        // Below is based on algebra of the intersection point of two lines
        t = point.x + (((point.y * lineVector.y) - (startingPoint.y * lineVector.y)) / lineVector.x) - startingPoint.x;
        t /= lineVector.x + (lineVector.y * lineVector.y / lineVector.x);

        t = Mathf.Clamp(t, 0, 1);
        return Vector3.Lerp(startingPoint, endingPoint, t);
    }

    public BezierContour Contour()
    {
        Vector2 midPoint = (startingPoint + endingPoint) / 2;
        var segments = new BezierPathSegment[]
        {
            new BezierPathSegment() { P0 = startingPoint, P1 = midPoint, P2 = endingPoint },
            new BezierPathSegment() { P0 = endingPoint }
        };
        return new BezierContour()
        {
            Segments = segments,
            Closed = false
        };
    }

    public float CompareToStroke(PenStroke stroke)
    {
        List<Vector2> points = stroke.Points;
        List<float> distances = new();
        float averageDistance = 0;
        foreach (Vector2 point in points)
        {
            Vector2 closestPointOnLine = ClosestPointOnLineTo(point);
            float distance = Vector2.Distance(point, closestPointOnLine);
            distances.Add(distance);
            averageDistance += distance;
        }
        averageDistance /= points.Count;
        return averageDistance;
    }
}
