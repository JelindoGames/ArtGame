using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

// A stroke of a raster pen.
public class PenStroke
{
    public List<Vector2> Points { get; private set; }

    public PenStroke(List<Vector2> points)
    {
        Points = points;
    }

    /// <summary>
    /// Goes through each point on the stroke to find which one is the closest
    /// to the given point.
    /// </summary>
    public Vector2 ClosestPointOnStrokeTo(Vector2 pointToCheck)
    {
        Vector2 closestPoint = Points[0];
        float closestDistance = float.MaxValue;
        foreach (Vector2 point in Points)
        {
            float distance = Vector2.Distance(pointToCheck, point);
            if (distance < closestDistance)
            {
                closestPoint = point;
                closestDistance = distance;
            }
        }
        return closestPoint;
    }
}
