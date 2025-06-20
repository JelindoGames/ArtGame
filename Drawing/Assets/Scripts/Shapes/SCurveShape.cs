using Unity.VectorGraphics;
using UnityEngine;

public class SCurveShape : IShape
{
    Vector2 startingPoint;
    Vector2 midPoint1;
    Vector2 midPoint2;
    Vector2 endingPoint;

    public SCurveShape(Vector2 startingPoint, Vector2 midPoint1, Vector2 midPoint2, Vector2 endingPoint)
    {
        this.startingPoint = startingPoint;
        this.midPoint1 = midPoint1;
        this.midPoint2 = midPoint2;
        this.endingPoint = endingPoint;
    }

    public BezierContour Contour()
    {
        var segments = new BezierPathSegment[]
        {
            new BezierPathSegment() { P0 = startingPoint, P1 = midPoint1, P2 = midPoint2 },
            new BezierPathSegment() { P0 = endingPoint }
        };
        return new BezierContour()
        {
            Segments = segments,
            Closed = false
        };
    }

    public string CompareToStroke(PenStroke stroke)
    {
        throw new System.NotImplementedException();
    }
}
