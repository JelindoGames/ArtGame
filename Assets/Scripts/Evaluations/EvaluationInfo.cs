using UnityEngine;

/// <summary>
/// Base interface for all evaluations.
/// Does not include a type argument, so can be used in place of Evaluation<V>
/// as long as you don't need to actually compare any raster and vector shapes.
/// </summary>
public interface EvaluationInfo
{
    /// <summary>
    /// Describes what this object is evaluating in a brief, 2-3 word string.
    /// </summary>
    public string Title();
}
