using UnityEngine;

/// <summary>
/// Base interface which describes an Evaluation.
/// Does not include a type argument, so can be used in place of Evaluation<V>
/// if you don't care what type of shape is being compared, and don't need to
/// actually compare any raster and vector shapes.
/// </summary>
public interface EvaluationInfo
{
    /// <summary>
    /// Describes what this object is evaluating in a brief, 2-3 word string.
    /// </summary>
    public string Title();
}
