using UnityEngine;

/// <summary>
/// Abstract class for a script that handles the logic for a lesson (a guided set of exercises for the user).
/// </summary>
public abstract class Lesson : ScriptableObject
{
    /// <summary>
    /// Sets the lesson in motion, showing the first exercise.
    /// </summary>
    public abstract void StartLesson(SpriteRenderer drawVectorShapesOn);

    /// <summary>
    /// React to the player's input, if necessary.
    /// </summary>
    public abstract void OnStroke(PenStroke stroke);
}
