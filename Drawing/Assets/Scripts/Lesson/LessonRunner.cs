using UnityEngine;

/// <summary>
/// Monobehaviour that holds the active lesson and listens to events from the scene.
/// </summary>
public class LessonRunner : MonoBehaviour
{
    [SerializeField] SpriteRenderer drawShapesOn;
    Lesson lesson;

    public void StartLesson(Lesson lesson)
    {
        this.lesson = lesson;
        lesson.StartLesson(drawShapesOn);
    }

    public void OnStroke(PenStroke stroke)
    {
        lesson.OnStroke(stroke);
    }
}
