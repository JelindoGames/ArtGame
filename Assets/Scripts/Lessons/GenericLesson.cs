using System.Collections.Generic;
using UnityEngine;
using TNRD;

/// <summary>
/// Handles the logic for a generic lesson.
/// </summary>
[CreateAssetMenu(menuName = "Lesson/Generic Lesson")]
public class GenericLesson : Lesson
{
    [SerializeField] SerializableInterface<ShapeGenerator<IShape>> shapeGenerator;
    [SerializeField] List<GenericEvaluation> evaluations;
    [SerializeField] int numberOfExercises;

    SpriteRenderer drawVectorShapesOn;
    Camera cam;

    bool lessonInSession;
    IShape currentShape;
    int currentExercise; // One-indexed

    protected override void OnLessonStart(SpriteRenderer drawVectorShapesOn)
    {
        cam = Camera.main;
        this.drawVectorShapesOn = drawVectorShapesOn;

        lessonInSession = true;
        currentExercise = 0;
        NextExercise();
    }

    void NextExercise()
    {
        currentExercise++;
        currentShape = shapeGenerator.Value.Generate(cam);
        ShapeDrawUtils.DrawShape(currentShape.Contour(), drawVectorShapesOn);
    }

    public override void OnStroke(PenStroke stroke)
    {
        if (!lessonInSession)
            return;

        foreach (GenericEvaluation eval in evaluations)
            Debug.Log(eval.Title() + ": " + eval.Compare(currentShape, stroke));

        if (currentExercise < numberOfExercises)
            NextExercise();
        else
            FinishLesson();
    }

    protected override void OnLessonQuit()
    {
        drawVectorShapesOn.sprite = null;
        lessonInSession = false;
    }
}
