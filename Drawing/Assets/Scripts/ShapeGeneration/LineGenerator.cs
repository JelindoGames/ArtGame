using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Generator/Line")]
public class LineGenerator : ShapeGenerator
{
    [SerializeField] float minLength;
    [SerializeField] float maxLength;

    public override BezierContour Generate(Camera referenceCam)
    {
        float angle = Random.Range(-Mathf.PI / 2, Mathf.PI / 2);
        float length = Random.Range(minLength, maxLength);
        Vector2 startingPoint = GenerateStartingPoint(angle, length, referenceCam);
        Vector2 endingPoint = new Vector2(startingPoint.x + (Mathf.Cos(angle) * length), startingPoint.y + (Mathf.Sin(angle) * length));
        Vector2 midPoint = (startingPoint + endingPoint) / 2;
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
