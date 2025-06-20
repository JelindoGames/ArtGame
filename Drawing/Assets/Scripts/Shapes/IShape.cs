using Unity.VectorGraphics;

/// <summary>
/// General interface for a drawable vector shape.
/// </summary>
public interface IShape
{
    /// <summary>
    /// Returns the Bezier contour that can be used to draw this shape.
    /// </summary>
    public BezierContour Contour();

    /// <summary>
    /// Returns how similar this shape is to the given stroke.
    /// Scored on a scale of 0 (completely different) to 1 (exactly the same).
    /// </summary>
    public float CompareToStroke(PenStroke stroke);
}
