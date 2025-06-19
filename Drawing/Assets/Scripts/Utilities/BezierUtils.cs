using UnityEngine;

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
    /// Brute-force method for roughly figuring out what the closest point on a cubic bezier curve is to a given point.
    /// Argument "measurements" represents how many points on the curve are checked.
    /// </summary>
    public static Vector2 ClosestPointOnCurve(Vector2 point, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int measurements=20)
    {
        Vector2 closestPoint = Vector2.zero;
        float closestDistance = float.MaxValue;
        for (float relativePos = 0; relativePos < 1; relativePos += 1f / measurements)
        {
            Vector2 curvePoint = CubicPoint(p0, p1, p2, p3, relativePos);
            float distance = Vector2.Distance(curvePoint, point);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = curvePoint;
            }
        }
        return closestPoint;
    }
}
