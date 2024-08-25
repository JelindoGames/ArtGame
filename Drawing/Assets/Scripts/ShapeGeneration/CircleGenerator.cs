using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Generator/Circle")]
public class CircleGenerator : ShapeGenerator
{
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;

    public override BezierContour Generate(Camera referenceCam)
    {
        Vector2 centerPoint = FindCenterPoint(referenceCam);
        float radius = Random.Range(minRadius, maxRadius);

        // Use a four-piece cubic bezier curve to APPROXIMATE a circle
        float distanceToOuterControlPoints = 0.5522847498f; // For a four-piece curve
        Vector2 s1p1 = centerPoint + new Vector2(1, -distanceToOuterControlPoints) * radius;
        Vector2 s1p2 = centerPoint + new Vector2(1, 0) * radius;
        Vector2 s1p3 = centerPoint + new Vector2(1, distanceToOuterControlPoints) * radius;
        Vector2 s2p1 = centerPoint + new Vector2(distanceToOuterControlPoints, 1) * radius;
        Vector2 s2p2 = centerPoint + new Vector2(0, 1) * radius;
        Vector2 s2p3 = centerPoint + new Vector2(-distanceToOuterControlPoints, 1) * radius;
        Vector2 s3p1 = centerPoint + new Vector2(-1, distanceToOuterControlPoints) * radius;
        Vector2 s3p2 = centerPoint + new Vector2(-1, 0) * radius;
        Vector2 s3p3 = centerPoint + new Vector2(-1, -distanceToOuterControlPoints) * radius;
        Vector2 s4p1 = centerPoint + new Vector2(-distanceToOuterControlPoints, -1) * radius;
        Vector2 s4p2 = centerPoint + new Vector2(0, -1) * radius;
        Vector2 s4p3 = centerPoint + new Vector2(distanceToOuterControlPoints, -1) * radius;

        var segments = new BezierPathSegment[]
        {
            new BezierPathSegment() { P0 = s1p2, P1 = s1p3, P2 = s2p1 },
            new BezierPathSegment() { P0 = s2p2, P1 = s2p3, P2 = s3p1 },
            new BezierPathSegment() { P0 = s3p2, P1 = s3p3, P2 = s4p1 },
            new BezierPathSegment() { P0 = s4p2, P1 = s4p3, P2 = s1p1 },
        };
        return new BezierContour()
        {
            Segments = segments,
            Closed = true
        };
    }

    Vector2 FindCenterPoint(Camera refCam)
    {
        Vector2 screenTopRight = refCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBottomLeft = refCam.ScreenToWorldPoint(new Vector2(0, 0));
        float bottomMostY = screenBottomLeft.y + maxRadius;
        float topMostY = screenTopRight.y - maxRadius;
        float leftMostX = screenBottomLeft.x + maxRadius;
        float rightMostX = screenTopRight.x - maxRadius;
        return new Vector2(Random.Range(leftMostX, rightMostX), Random.Range(bottomMostY, topMostY));
    }
}
