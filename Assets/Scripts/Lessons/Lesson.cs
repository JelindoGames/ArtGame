using System;
using UnityEngine;

/// <summary>
/// Abstract class which handles all high-level lesson logic.
/// </summary>
public abstract class Lesson : ScriptableObject
{
    Action onLessonFinishCallback;

    /// <summary>
    /// Starts the lesson and shows the first exercise.
    /// </summary>
    public void StartLesson(SpriteRenderer drawVectorShapesOn, Action onLessonFinishCallback)
    {
        this.onLessonFinishCallback = onLessonFinishCallback;
        OnLessonStart(drawVectorShapesOn);
    }

    /// <summary>
    /// Starts the lesson and shows the first exercise.
    /// </summary>
    protected abstract void OnLessonStart(SpriteRenderer drawVectorShapesOn);

    /// <summary>
    /// Forcibly ends the lesson. Stops showing any currently displayed exercise.
    /// </summary>
    public void QuitLesson()
    {
        onLessonFinishCallback = null;
        OnLessonQuit();
    }

    /// <summary>
    /// Forcibly ends the lesson. Stops showing any currently displayed exercise.
    /// </summary>
    protected abstract void OnLessonQuit();

    /// <summary>
    /// To be called internally to signal that the lesson has finished naturally.
    /// </summary>
    protected void FinishLesson()
    {
        onLessonFinishCallback.Invoke();
        onLessonFinishCallback = null;
    }

    /// <summary>
    /// React to the player's input, if necessary.
    /// </summary>
    public abstract void OnStroke(PenStroke stroke);
}
