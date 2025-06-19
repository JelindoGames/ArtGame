using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;
using UnityAtoms.BaseAtoms;
using TMPro;

public class Lesson : MonoBehaviour
{
    [SerializeField] ShapeGenerator shapeGenerator;
    [SerializeField] SpriteRenderer drawShapesOn;
    [SerializeField] bool playOnStart;
    [SerializeField] VoidEvent onLessonStart;
    [SerializeField] TextMeshProUGUI performanceText;
    Camera cam;
    BezierContour currentContour;

    void Start()
    {
        cam = Camera.main;
        if (playOnStart)
        {
            StartExercise();
        }
    }

    void StartExercise()
    {
        onLessonStart.Raise();
        currentContour = shapeGenerator.Generate(cam);
        ShapeDrawUtils.DrawShape(currentContour, drawShapesOn);
    }

    public void ReviewExercise(PenStroke stroke)
    {
        performanceText.text = "Score: " + stroke.CompareWithBezier(currentContour);
        StartExercise();
    }
}
