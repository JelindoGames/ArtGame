using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Generator/Ellipse")]
public class EllipseGenerator : ShapeGenerator
{
    [SerializeField] float minSizeX;
    [SerializeField] float maxSizeX;
    [SerializeField] float minSizeY;
    [SerializeField] float maxSizeY;

    public override BezierContour Generate(Camera referenceCam)
    {
        Vector2 centerPoint = FindCenterPoint(referenceCam);
        float xSize = Random.Range(minSizeX, maxSizeX);
        float ySize = Random.Range(minSizeY, maxSizeY);
        float angle = Random.Range(0, Mathf.PI * 2);

        // Use a four-piece cubic bezier curve to APPROXIMATE a circle
        float distanceToOuterControlPoints = 0.5522847498f; // For a four-piece curve
        Vector2 s1p1 = RotatePointByAngle(new Vector2(xSize, -distanceToOuterControlPoints * ySize), angle) + centerPoint;
        Vector2 s1p2 = RotatePointByAngle(new Vector2(xSize, 0), angle) + centerPoint;
        Vector2 s1p3 = RotatePointByAngle(new Vector2(xSize, distanceToOuterControlPoints * ySize), angle) + centerPoint;
        Vector2 s2p1 = RotatePointByAngle(new Vector2(distanceToOuterControlPoints * xSize, ySize), angle) + centerPoint;
        Vector2 s2p2 = RotatePointByAngle(new Vector2(0, ySize), angle) + centerPoint;
        Vector2 s2p3 = RotatePointByAngle(new Vector2(-distanceToOuterControlPoints * xSize, ySize), angle) + centerPoint;
        Vector2 s3p1 = RotatePointByAngle(new Vector2(-xSize, distanceToOuterControlPoints * ySize), angle) + centerPoint;
        Vector2 s3p2 = RotatePointByAngle(new Vector2(-xSize, 0), angle) + centerPoint;
        Vector2 s3p3 = RotatePointByAngle(new Vector2(-xSize, -distanceToOuterControlPoints * ySize), angle) + centerPoint;
        Vector2 s4p1 = RotatePointByAngle(new Vector2(-distanceToOuterControlPoints * xSize, -ySize), angle) + centerPoint;
        Vector2 s4p2 = RotatePointByAngle(new Vector2(0, -ySize), angle) + centerPoint;
        Vector2 s4p3 = RotatePointByAngle(new Vector2(distanceToOuterControlPoints * xSize, -ySize), angle) + centerPoint;

        var segments = new BezierPathSegment[]
        {
            new BezierPathSegment() { P0 = s1p2, P1 = s1p3, P2 = s2p1 },
            new BezierPathSegment() { P0 = s2p2, P1 = s2p3, P2 = s3p1 },
            new BezierPathSegment() { P0 = s3p2, P1 = s3p3, P2 = s4p1 },
            new BezierPathSegment() { P0 = s4p2, P1 = s4p3, P2 = s1p1 },
            new BezierPathSegment() { P0 = s1p2 }
        };
        return new BezierContour()
        {
            Segments = segments,
            Closed = true
        };
    }

    Vector2 RotatePointByAngle(Vector2 origPoint, float angleChange)
    {
        float origPointAngle = Mathf.Atan2(origPoint.y, origPoint.x);
        float newPointAngle = origPointAngle + angleChange;
        return origPoint.magnitude * new Vector2(Mathf.Cos(newPointAngle), Mathf.Sin(newPointAngle));
    }

    Vector2 FindCenterPoint(Camera refCam)
    {
        Vector2 screenTopRight = refCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBottomLeft = refCam.ScreenToWorldPoint(new Vector2(0, 0));
        float maxSizeGeneral = Mathf.Max(maxSizeX, maxSizeY);
        float bottomMostY = screenBottomLeft.y + maxSizeGeneral;
        float topMostY = screenTopRight.y - maxSizeGeneral;
        float leftMostX = screenBottomLeft.x + maxSizeGeneral;
        float rightMostX = screenTopRight.x - maxSizeGeneral;
        return new Vector2(Random.Range(leftMostX, rightMostX), Random.Range(bottomMostY, topMostY));
    }
}
