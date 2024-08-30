using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

public static class ShapeDrawUtils
{
    public static void DrawShape(BezierContour contour, SpriteRenderer renderer)
    {
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
        var tessellationOptions = new VectorUtils.TessellationOptions() { StepDistance = 0.1f, MaxCordDeviation = float.MaxValue, MaxTanAngleDeviation = float.MaxValue, SamplingStepSize = 0.5f };
        List<VectorUtils.Geometry> geometries = VectorUtils.TessellateScene(scene, tessellationOptions);
        Sprite sprite = VectorUtils.BuildSprite(geometries, 1f, VectorUtils.Alignment.SVGOrigin, Vector2.zero, 1);
        renderer.sprite = sprite;
    }
}
