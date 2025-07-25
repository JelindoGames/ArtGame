using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents user performance data for a completed lesson.
/// </summary>
public class LessonResult
{
    Dictionary<EvaluationInfo, List<float>> scores;

    public LessonResult()
    {
        scores = new();
    }

    /// <summary>
    /// Adds a single evaluation's output to this set of results.
    /// </summary>
    public void AddScore(EvaluationInfo evaluationInfo, float score)
    {
        if (!scores.ContainsKey(evaluationInfo))
        {
            scores[evaluationInfo] = new List<float>();
        }
        scores[evaluationInfo].Add(score);
    }

    /// <summary>
    /// Retrieves all the evaluation scores from this lesson.
    /// </summary>
    public Dictionary<EvaluationInfo, List<float>> GetScores()
    {
        return new Dictionary<EvaluationInfo, List<float>>(scores);
    }
}
