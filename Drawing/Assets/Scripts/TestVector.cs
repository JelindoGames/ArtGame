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
        /*
        var segments = new BezierPathSegment[] {
            new BezierPathSegment() { P0 = a.position, P1 = b.position, P2 = c.position },
            new BezierPathSegment() { P0 = d.position, P1 = e.position, P2 = f.position },
            new BezierPathSegment() { P0 = g.position }
        };
        contour = new BezierContour()
        {
            Segments = segments,
            Closed = false
        };
        */
        contour = shapeGenerator.Generate(referenceCam);
        var pathProps = new PathProperties()
        {
            Stroke = new Stroke() { Color = Color.red, HalfThickness = 0.1f }
        };
        var shape = new Shape()
        {
            Contours = new BezierContour[] { contour },
            //Fill = new SolidFill() { Color = Color.red, Mode = FillMode.NonZero },
            PathProps = pathProps
        };
        var sceneNode = new SceneNode()
        {
            Shapes = new List<Shape>() { shape }
        };
        var scene = new Scene()
        {
            Root = sceneNode
        };
        var tessellationOptions = new VectorUtils.TessellationOptions() { StepDistance = 0.1f, MaxCordDeviation = float.MaxValue, MaxTanAngleDeviation = float.MaxValue, SamplingStepSize = 0.5f};
        List<VectorUtils.Geometry> geometries = VectorUtils.TessellateScene(scene, tessellationOptions);
        Sprite sprite = VectorUtils.BuildSprite(geometries, 1f, VectorUtils.Alignment.SVGOrigin, Vector2.zero, 1);
        spriteRenderer.sprite = sprite;
    }
}
