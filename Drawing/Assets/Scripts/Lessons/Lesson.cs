using UnityEngine;

/// <summary>
/// Class that handles the logic for a lesson (a guided set of exercises for the user).
/// </summary>
public abstract class Lesson : ScriptableObject
{
    public abstract void StartLesson(SpriteRenderer drawVectorShapesOn);

    public abstract void OnStroke(PenStroke stroke);
}
