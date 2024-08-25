using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Generator/S Curve")]
public class SCurveGenerator : ShapeGenerator
{
    [SerializeField] float minLength;
    [SerializeField] float maxLength;
    [SerializeField] float minMidpointDistance;
    [SerializeField] float maxMidpointDistance;

    public override BezierContour Generate(Camera referenceCam)
    {
        float angle = Random.Range(-Mathf.PI / 2, Mathf.PI / 2);
        float length = Random.Range(minLength, maxLength);
        Vector2 startingPoint = GenerateStartingPoint(angle, length, referenceCam);
        Vector2 endingPoint = new Vector2(startingPoint.x + (Mathf.Cos(angle) * length), startingPoint.y + (Mathf.Sin(angle) * length));
        var midPoints = GenerateMidPoints(startingPoint, endingPoint, referenceCam);
        var segments = new BezierPathSegment[]
        {
            new BezierPathSegment() { P0 = startingPoint, P1 = midPoints.Item1, P2 = midPoints.Item2 },
            new BezierPathSegment() { P0 = endingPoint }
        };
        return new BezierContour()
        {
            Segments = segments,
            Closed = false
        };
    }

    System.Tuple<Vector2, Vector2> GenerateMidPoints(Vector2 startingPoint, Vector2 endPoint, Camera referenceCam)
    {
        Vector2 linearMidPoint1 = Vector2.Lerp(startingPoint, endPoint, 0.333f);
        Vector2 linearMidPoint2 = Vector2.Lerp(startingPoint, endPoint, 0.666f);

        float curveMidPointDist = Random.Range(minMidpointDistance, maxMidpointDistance);
        float curveMidPointAngle = Random.Range(0, 2 * Mathf.PI);

        Vector2 curveMidPoint1 = linearMidPoint1 + (new Vector2(Mathf.Cos(curveMidPointAngle), Mathf.Sin(curveMidPointAngle)) * curveMidPointDist);
        curveMidPointAngle += Mathf.PI;
        Vector2 curveMidPoint2 = linearMidPoint2 + (new Vector2(Mathf.Cos(curveMidPointAngle), Mathf.Sin(curveMidPointAngle)) * curveMidPointDist);

        return new System.Tuple<Vector2, Vector2>(curveMidPoint1, curveMidPoint2);
    }

    Vector2 GenerateStartingPoint(float lineAngle, float lineLength, Camera referenceCam)
    {
        float sin = Mathf.Sin(lineAngle);
        float cos = Mathf.Cos(lineAngle);
        Vector2 screenTopRight = referenceCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBottomLeft = referenceCam.ScreenToWorldPoint(new Vector2(0, 0));
        float rightmostStartingX = screenTopRight.x - (cos * lineLength) - maxMidpointDistance;
        float leftmostStartingX = screenBottomLeft.x + maxMidpointDistance;
        if (sin > 0)
        {
            float topMostStartingY = screenTopRight.y - (sin * lineLength) - maxMidpointDistance;
            float bottomMostStartingY = screenBottomLeft.y + maxMidpointDistance;
            return new Vector2(Random.Range(leftmostStartingX, rightmostStartingX), Random.Range(bottomMostStartingY, topMostStartingY));
        }
        else
        {
            float bottomMostStartingY = screenBottomLeft.y - (sin * lineLength) + maxMidpointDistance;
            float topMoststartingY = screenTopRight.y - maxMidpointDistance;
            return new Vector2(Random.Range(leftmostStartingX, rightmostStartingX), Random.Range(bottomMostStartingY, topMoststartingY));
        }
    }
}
