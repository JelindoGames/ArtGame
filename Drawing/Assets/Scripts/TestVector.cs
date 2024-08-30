using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

public class TestVector : MonoBehaviour
{
    [SerializeField] Transform a, b, c, d, e, f, g;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ShapeGenerator shapeGenerator;
    [SerializeField] Camera referenceCam;
    public BezierContour contour;

    void Start()
    {
        contour = shapeGenerator.Generate(referenceCam);
        ShapeDrawUtils.DrawShape(contour, spriteRenderer);
    }
}
