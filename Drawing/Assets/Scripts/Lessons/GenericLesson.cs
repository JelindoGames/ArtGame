using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the logic for a line-drawing lesson.
/// </summary>
[CreateAssetMenu(menuName = "Lesson/LineLesson")]
public class GenericLesson : Lesson
{
    [SerializeField] ShapeGenerator shapeGenerator;
    [SerializeField] List<GenericEvaluation> evaluations;
    SpriteRenderer drawVectorShapesOn;
    IShape currentShape;
    Camera cam;

    public override void StartLesson(SpriteRenderer drawVectorShapesOn)
    {
        cam = Camera.main;
        this.drawVectorShapesOn = drawVectorShapesOn;
        StartExercise();
    }

    void StartExercise()
    {
        currentShape = shapeGenerator.Generate(cam);
        ShapeDrawUtils.DrawPoints(currentShape.DefiningPoints(), drawVectorShapesOn);
    }

    public override void OnStroke(PenStroke stroke)
    {
        //Debug.Log(stroke.CompareWithShape(currentShape));
        foreach (GenericEvaluation eval in evaluations)
        {
            Debug.Log(eval.Compare(currentShape, stroke));
        }
        StartExercise();
    }
}
