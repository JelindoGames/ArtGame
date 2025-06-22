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
    /// Returns t, the point on the line where 0 is startingPoint and 1 is endingPoint,
    /// which is closet to the given point. The returned number can be less than 0
    /// or greater than 1.
    /// </summary>
    float ClosestPointOnShapeUnbounded(Vector2 point)
    {
        Vector2 lineVector = endingPoint - startingPoint;
        if (lineVector.x == 0) // Vertical line case
        {
            return (point.y - startingPoint.y) / (endingPoint.y - startingPoint.y);
        }
        if (lineVector.y == 0) // Horizontal line case
        {
            return (point.x - startingPoint.x) / (endingPoint.x - startingPoint.x);
        }

        float t; // The spot on this line (where 0 = startingPoint to 1 = endingPoint) which is closest
        // Below is based on algebra of the intersection point of two lines
        t = point.x + (((point.y * lineVector.y) - (startingPoint.y * lineVector.y)) / lineVector.x) - startingPoint.x;
        t /= lineVector.x + (lineVector.y * lineVector.y / lineVector.x);
        return t;
    }

    // Override default IShape implementation for efficiency
    public Vector2 ClosestPointOnShapeTo(Vector2 point)
    {
        float t = Mathf.Clamp(ClosestPointOnShapeUnbounded(point), 0, 1);
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
        for (int i = 1; i < 10; i++)
        {
            int a = distances.Count * (i - 1) / 10;
            int b = distances.Count * i / 10;
            shakiness += Mathf.Abs(distances[b] - distances[a]);
        }
        shakiness /= 10;

        Vector2 closestToStartingPoint = stroke.ClosestPointOnStrokeTo(startingPoint);
        Vector2 closestToEndingPoint = stroke.ClosestPointOnStrokeTo(endingPoint);
        float closestDistanceToStart = Vector2.Distance(closestToStartingPoint, startingPoint);
        float closestDistanceToEnd = Vector2.Distance(closestToEndingPoint, endingPoint);

        return $"AVG: {averageDistance}\nSHAKINESS: {shakiness}\nSTART DIST: {closestDistanceToStart}\nEND DIST: {closestDistanceToEnd}";
    }
}
