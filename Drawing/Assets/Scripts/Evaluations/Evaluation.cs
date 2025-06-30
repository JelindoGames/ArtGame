using UnityEngine;

/// <summary>
/// Evaluates a user's raster shape in comparison to a vector shape.
/// </summary>
public abstract class Evaluation<V> : ScriptableObject where V : IShape
{
    /// <summary>
    /// Evaluates the given raster shape's similarity to the given vector shape, on a scale
    /// from 0 (similar) to 1 (not similar)
    /// </summary>
    public abstract float Compare(V vector, PenStroke raster);
}
