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
    IShape currentShape;

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
        currentShape = shapeGenerator.Generate(cam);
        ShapeDrawUtils.DrawShape(currentShape.Contour(), drawShapesOn);
    }

    public void ReviewExercise(PenStroke stroke)
    {
        performanceText.text = stroke.CompareWithShape(currentShape);
        //StartExercise();
    }
}
