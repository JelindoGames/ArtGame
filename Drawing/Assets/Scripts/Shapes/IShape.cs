using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

/// <summary>
/// General interface for a drawable vector shape.
/// </summary>
public interface IShape
{
    /// <summary>
    /// Returns the Bezier contour that can be used to draw this shape.
    /// </summary>
    public BezierContour Contour();

    /// <summary>
    /// Finds the closest point on this shape to the given point.
    /// </summary>
    public Vector2 ClosestPointOnShapeTo(Vector2 point)
    {
        BezierContour contour = Contour();
        List<Vector2> keyBezierPoints = new();
        foreach (BezierPathSegment segment in contour.Segments)
        {
            keyBezierPoints.Add(segment.P0);
            keyBezierPoints.Add(segment.P1);
            keyBezierPoints.Add(segment.P2);
        }

        // For each cubic bezier curve in the overall contour, find the closest points to the given point
        List<Vector2> bezierPoints = new();
        for (int i = 0; i < keyBezierPoints.Count - 3; i++)
        {
            bezierPoints.AddRange(BezierUtils.GetRepresentativePoints(keyBezierPoints[i], keyBezierPoints[i + 3], keyBezierPoints[i + 1], keyBezierPoints[i + 2], 20));
        }
        return Vector2.zero;
    }

    /// <summary>
    /// Returns how similar this shape is to the given stroke.
    /// Scored on a scale of 0 (completely different) to 1 (exactly the same).
    /// </summary>
    public string CompareToStroke(PenStroke stroke);
}
