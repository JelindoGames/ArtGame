using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;
using MathNet.Numerics;

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
    public UnityEngine.Vector2 ClosestPointOnShapeTo(UnityEngine.Vector2 point)
    {
        BezierContour contour = Contour();
        List<UnityEngine.Vector2> keyBezierPoints = new();
        foreach (BezierPathSegment segment in contour.Segments)
        {
            keyBezierPoints.Add(segment.P0);
            keyBezierPoints.Add(segment.P1);
            keyBezierPoints.Add(segment.P2);
        }

        float minDistance = float.MaxValue;
        UnityEngine.Vector2 closestPoint = UnityEngine.Vector2.zero;

        // For each cubic bezier curve in the overall contour, find the closest points to the given point
        for (int i = 0; i < keyBezierPoints.Count - 3; i+= 3)
        {
            // Key points of the cubic curve
            UnityEngine.Vector2 p0 = keyBezierPoints[i];
            UnityEngine.Vector2 p1 = keyBezierPoints[i + 1];
            UnityEngine.Vector2 p2 = keyBezierPoints[i + 2];
            UnityEngine.Vector2 p3 = keyBezierPoints[i + 3];
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
            float dc = 4 * ((2 * ((ax * cx) + (ay * cy))) + Mathf.Pow(bx, 2) + Mathf.Pow(by, 2));
            float dd = 6 * ((ax * (dx - point.x)) + (bx * cx) + (ay * (dy - point.y)) + (by * cy));
            float de = 2 * ((2 * ((bx * dx) - (bx * point.x) + (by * dy) - (by * point.y))) + Mathf.Pow(cx, 2) + Mathf.Pow(cy, 2));
            float df = 2 * ((cx * dx) - (cx * point.x) + (cy * dy) - (cy * point.y));
            // Find roots of distance derivative function, filter out any imaginary results
            Complex[] derivativeRoots = FindRoots.Polynomial(new double[] { df, de, dd, dc, db, da });
            List<double> realRoots = new();
            foreach (Complex root in derivativeRoots)
            {
                if (Mathf.Abs((float)root.Imaginary) < 0.1f)
                {
                    realRoots.Add(root.Real);
                }
            }
            realRoots.Add(0); // Add the start and end of the curve just in case those are closest (but not a local minimum)
            realRoots.Add(1);
            // Each derivative root can be a local minimum. Find distance there to see if it's the min distance
            foreach (float t in realRoots)
            {
                if (t < 0 || t > 1) continue;
                float bezierPointX = (ax * Mathf.Pow(t, 3)) + (bx * Mathf.Pow(t, 2)) + (cx * t) + dx;
                float bezierPointY = (ay * Mathf.Pow(t, 3)) + (by * Mathf.Pow(t, 2)) + (cy * t) + dy;
                float distanceX = bezierPointX - point.x;
                float distanceY = bezierPointY - point.y;
                float distance = Mathf.Sqrt(Mathf.Pow(distanceX, 2) + Mathf.Pow(distanceY, 2));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = new UnityEngine.Vector2(bezierPointX, bezierPointY);
                }
            }
        }
        return closestPoint;
    }

    /// <summary>
    /// Returns how similar this shape is to the given stroke.
    /// Scored on a scale of 0 (completely different) to 1 (exactly the same).
    /// </summary>
    public string CompareToStroke(PenStroke stroke)
    {
        float averageDistance = 0;
        foreach (UnityEngine.Vector2 point in stroke.Points)
        {
            UnityEngine.Vector2 closestPoint = ClosestPointOnShapeTo(point);
            averageDistance += UnityEngine.Vector2.Distance(point, closestPoint);
        }
        averageDistance /= stroke.Points.Count;
        return $"AVG DIST: {averageDistance}";
    }
}
