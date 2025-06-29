using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

public static class ShapeDrawUtils
{
    /// <summary>
    /// Draws this shape on the given sprite renderer.
    /// </summary>
    public static void DrawShape(BezierContour contour, SpriteRenderer renderer)
    {
        var pathProps = new PathProperties()
        {
            Stroke = new Stroke() { Color = new Color(1, 0, 0, 0.25f), HalfThickness = 0.1f }
        };
        var shape = new Shape()
        {
            Contours = new BezierContour[] { contour },
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
        var tessellationOptions = new VectorUtils.TessellationOptions() { StepDistance = 0.01f, MaxCordDeviation = float.MaxValue, MaxTanAngleDeviation = float.MaxValue, SamplingStepSize = 0.5f };
        List<VectorUtils.Geometry> geometries = VectorUtils.TessellateScene(scene, tessellationOptions);
        Sprite sprite = VectorUtils.BuildSprite(geometries, 1f, VectorUtils.Alignment.SVGOrigin, Vector2.zero, 1);
        renderer.sprite = sprite;
    }

    /// <summary>
    /// Draws the key Bezier points of this shape on the given sprite renderer.
    /// </summary>
    public static void DrawPoints(List<Vector2> points, SpriteRenderer renderer)
    {
        var pathProps = new PathProperties()
        {
            Stroke = new Stroke() { Color = new Color(0, 0, 0, 0), HalfThickness = 0.1f },
            Head = PathEnding.Round
        };
        List<Shape> shapesToDraw = new();
        foreach (Vector2 point in points)
        {
            BezierContour contourToDraw = new CircleShape(point, 0.2f).Contour();
            var shapeToDraw = new Shape()
            {
                Contours = new BezierContour[] { contourToDraw },
                Fill = new SolidFill() { Color = new Color(1, 0, 0, 0.25f), Mode = FillMode.NonZero },
                PathProps = pathProps
            };
            shapesToDraw.Add(shapeToDraw);
        }
        var sceneNode = new SceneNode()
        {
            Shapes = shapesToDraw
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
