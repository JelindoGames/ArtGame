using Unity.VectorGraphics;
using UnityEngine;

public class CCurveShape : IShape
{
    Vector2 startingPoint;
    Vector2 midPoint;
    Vector2 endingPoint;

    public CCurveShape(Vector2 startingPoint, Vector2 midPoint, Vector2 endingPoint)
    {
        this.startingPoint = startingPoint;
        this.midPoint = midPoint;
        this.endingPoint = endingPoint;
    }

    public BezierContour Contour()
    {
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

    public string CompareToStroke(PenStroke stroke)
    {
        throw new System.NotImplementedException();
    }
}
