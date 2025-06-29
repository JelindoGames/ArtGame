using UnityEngine;

/// <summary>
/// Handles the logic for a line-drawing lesson.
/// </summary>
[CreateAssetMenu(menuName = "Lesson/LineLesson")]
public class GenericLesson : Lesson
{
    [SerializeField] ShapeGenerator shapeGenerator;
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
        Debug.Log(stroke.CompareWithShape(currentShape));
        StartExercise();
    }
}
