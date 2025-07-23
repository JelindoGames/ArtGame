using UnityEngine;

/// <summary>
/// Interface for a class that can generate a bezier shape (min complexity cubic).
/// </summary>
public interface ShapeGenerator<out S> where S : IShape
{
    /// <summary>
    /// Generate the bezier shape (min complexity cubic).
    /// </summary>
    /// <returns>The shape, with its segments and whether it's closed.</returns>
    public abstract S Generate(Camera referenceCam);
}
