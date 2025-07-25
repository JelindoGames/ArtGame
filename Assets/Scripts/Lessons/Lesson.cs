using System;
using UnityEngine;

/// <summary>
/// Abstract class which handles all high-level lesson logic.
/// </summary>
public abstract class Lesson : ScriptableObject
{
    Action<LessonResult> onLessonFinishCallback;

    /// <summary>
    /// Starts the lesson and shows the first exercise.
    /// The given callback will be called when the lesson finishes naturally.
    /// </summary>
    public void StartLesson(SpriteRenderer drawVectorShapesOn, Action<LessonResult> onLessonFinishCallback)
    {
        this.onLessonFinishCallback = onLessonFinishCallback;
        OnLessonStart(drawVectorShapesOn);
    }

    /// <summary>
    /// Starts the lesson and shows the first exercise.
    /// </summary>
    protected abstract void OnLessonStart(SpriteRenderer drawVectorShapesOn);

    /// <summary>
    /// Ends the lesson, halting all progress and hiding lesson graphics.
    /// This is *not* considered a natural end, and does *not* call the onLessonFinish callback.
    /// </summary>
    public void QuitLesson()
    {
        onLessonFinishCallback = null;
        OnLessonQuit();
    }

    /// <summary>
    /// Ends the lesson, halting all progress and hiding lesson graphics.
    /// </summary>
    protected abstract void OnLessonQuit();

    /// <summary>
    /// Ends the lesson naturally. To be called internally when the lesson is over.
    /// This also quits the lesson.
    /// </summary>
    protected void FinishLesson(LessonResult result)
    {
        onLessonFinishCallback.Invoke(result);
        QuitLesson();
    }

    /// <summary>
    /// React to the player's input, if necessary.
    /// </summary>
    public abstract void OnStroke(PenStroke stroke);
}
