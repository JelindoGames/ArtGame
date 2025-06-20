using Unity.VectorGraphics;
using UnityEngine;

public class LineShape : IShape
{
    Vector2 startingPoint;
    Vector2 endingPoint;

    public LineShape(Vector2 startingPoint, Vector2 endingPoint)
    {
        this.startingPoint = startingPoint;
        this.endingPoint = endingPoint;
    }

    public BezierContour Contour()
    {
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

    public float CompareToStroke(PenStroke stroke)
    {
        throw new System.NotImplementedException();
    }
}
