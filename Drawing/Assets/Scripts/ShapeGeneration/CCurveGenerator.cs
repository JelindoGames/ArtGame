using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Generator/C Curve")]
public class CCurveGenerator : ShapeGenerator
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
        Vector2 midPoint = GenerateMidPoint(startingPoint, endingPoint, referenceCam);
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

    Vector2 GenerateMidPoint(Vector2 startingPoint, Vector2 endPoint, Camera referenceCam)
    {
        Vector2 linearMidPoint = (startingPoint + endPoint) / 2;
        Vector2 screenTopRight = referenceCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBottomLeft = referenceCam.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 curveMidPoint;
        do
        {
            float dist = Random.Range(minMidpointDistance, maxMidpointDistance);
            float angle = Random.Range(0, 2 * Mathf.PI);
            curveMidPoint = linearMidPoint + (new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist);
        }
        while (curveMidPoint.x < screenBottomLeft.x || curveMidPoint.y < screenBottomLeft.y || curveMidPoint.x > screenTopRight.x || curveMidPoint.y > screenTopRight.y);
        return curveMidPoint;
    }

    Vector2 GenerateStartingPoint(float lineAngle, float lineLength, Camera referenceCam)
    {
        float sin = Mathf.Sin(lineAngle);
        float cos = Mathf.Cos(lineAngle);
        Vector2 screenTopRight = referenceCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBottomLeft = referenceCam.ScreenToWorldPoint(new Vector2(0, 0));
        float rightmostStartingX = screenTopRight.x - (cos * lineLength);
        if (sin > 0)
        {
            float topmostStartingY = screenTopRight.y - (sin * lineLength);
            return new Vector2(Random.Range(screenBottomLeft.x, rightmostStartingX), Random.Range(screenBottomLeft.y, topmostStartingY));
        }
        else
        {
            float bottomMostStartingY = screenBottomLeft.y - (sin * lineLength);
            return new Vector2(Random.Range(screenBottomLeft.x, rightmostStartingX), Random.Range(bottomMostStartingY, screenTopRight.y));
        }
    }
}
