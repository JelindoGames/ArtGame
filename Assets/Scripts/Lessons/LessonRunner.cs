using UnityEngine;

/// <summary>
/// Monobehaviour that holds the active lesson and listens to events from the scene.
/// </summary>
public class LessonRunner : MonoBehaviour
{
    [SerializeField] SpriteRenderer drawShapesOn;
    Lesson currentLesson;

    public void StartLesson(Lesson newLesson)
    {
        if (currentLesson != null)
        {
            currentLesson.QuitLesson();
        }
        currentLesson = newLesson;
        currentLesson.StartLesson(drawShapesOn, OnLessonFinish);
    }

    void OnLessonFinish()
    {
        print("LessonRunner: The lesson told me it's over!");
    }

    public void OnStroke(PenStroke stroke)
    {
        currentLesson.OnStroke(stroke);
    }
}
