using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

/// <summary>
/// Encompasses any class that can generate (either randomly or based on something)
/// a bezier shape (min complexity cubic).
/// </summary>
public abstract class ShapeGenerator : ScriptableObject
{
    /// <summary>
    /// Generate the bezier shape (min complexity cubic).
    /// </summary>
    /// <returns>The shape, with its segments and whether it's closed.</returns>
    public abstract BezierContour Generate(Camera referenceCam);
}
