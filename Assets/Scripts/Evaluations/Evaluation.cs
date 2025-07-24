using UnityEngine;

/// <summary>
/// Object which compares a raster shape to a vector shape in some specific way.
/// 
/// NOTE: For ease of inspector selector use, you shouldn't directly inherit from this class!
/// Instead, make a class which inherits from Evaluation<SomeSpecificClass>,
/// and then inherit from that.
/// </summary>
public abstract class Evaluation<V> : ScriptableObject where V : IShape
{
    [SerializeField] string title;

    /// <summary>
    /// Describes what this object is evaluating in a brief, 2-3 word string.
    /// </summary>
    public string Title() => title;

    /// <summary>
    /// Evaluates the given raster shape's similarity to the given vector shape, on a scale
    /// from 0 (similar) to 1 (not similar).
    /// </summary>
    public abstract float Compare(V vector, PenStroke raster);
}
