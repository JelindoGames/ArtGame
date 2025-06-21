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
            // Key points of the cubic curve
            Vector2 p0 = keyBezierPoints[i];
            Vector2 p1 = keyBezierPoints[i + 1];
            Vector2 p2 = keyBezierPoints[i + 2];
            Vector2 p3 = keyBezierPoints[i + 3];
            // Coefficients of the cubic bezier curve equation, when translated to standard polynomial form
            float ax = -p0.x + (3 * p1.x) - (3 * p2.x) + p3.x;
            float bx = (3 * p0.x) - (6 * p1.x) + (3 * p2.x);
            float cx = (-3 * p0.x) + (3 * p1.x);
            float dx = p0.x;
            float ay = -p0.y + (3 * p1.y) - (3 * p2.y) + p3.y;
            float by = (3 * p0.y) - (6 * p1.y) + (3 * p2.y);
            float cy = (-3 * p0.y) + (3 * p1.y);
            float dy = p0.y;
            // Coefficients of the distance derivative, when translated to standard polynomial form
            float da = 6 * (Mathf.Pow(ax, 2) + Mathf.Pow(ay, 2));
            float db = 10 * ((ax * bx) + (ay * by));
            float dc = 4 * ((2 * ((ax * cx) + (ay * by))) + Mathf.Pow(bx, 2) + Mathf.Pow(by, 2));
            float dd = 6 * ((ax * (dx - point.x)) + (bx * cx) + (ay * (dy - point.y)) + (by * cy));
            float de = 2 * ((2 * ((bx * dx) - (bx * point.x) + (by * dy) - (by * point.y))) + Mathf.Pow(cx, 2) + Mathf.Pow(cy, 2));
            float df = 2 * ((cx * dx) - (cx * point.x) + (cy * dy) - (cy * point.y));

        }
        return Vector2.zero;
    }

    /// <summary>
    /// Returns how similar this shape is to the given stroke.
    /// Scored on a scale of 0 (completely different) to 1 (exactly the same).
    /// </summary>
    public string CompareToStroke(PenStroke stroke);
}
