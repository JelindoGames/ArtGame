using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Custom-written utilities for dealing with Bezier curves.
/// </summary>
public static class BezierUtils
{
    /// <summary>
    /// Returns the point on a cubic bezier curve with points p0, p1, p2, and p3.
    /// Argument "t" represents the relative position on the curve, from 0 to 1.
    /// </summary>
    public static Vector2 CubicPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        if (t < 0 || t > 1)
        {
            Debug.LogError("BezierUtils: t must be between 0 and 1.");
        }
        return p0 * Mathf.Pow(1 - t, 3) +
            p1 * 3 * Mathf.Pow(1 - t, 2) * t +
            p2 * 3 * (1 - t) * Mathf.Pow(t, 2) +
            p3 * Mathf.Pow(t, 3);
    }

    /// <summary>
    /// Get "amount" points on a Bezier Curve, which are evenly spaced from each other.
    /// </summary>
    public static List<Vector2> GetRepresentativePoints(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int amount)
    {
        List<Vector2> toReturn = new();
        for (int i = 0; i < amount; i++)
        {
            float progress = (float)i / amount;
            toReturn.Add(CubicPoint(p0, p1, p2, p3, progress));
        }
        return toReturn;
    }
}
