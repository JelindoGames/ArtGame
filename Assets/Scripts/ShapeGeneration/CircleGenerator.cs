using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Generator/Circle")]
public class CircleGenerator : ScriptableObject, ShapeGenerator<CircleShape>
{
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;

    public CircleShape Generate(Camera referenceCam)
    {
        Vector2 centerPoint = FindCenterPoint(referenceCam);
        float radius = Random.Range(minRadius, maxRadius);
        return new CircleShape(centerPoint, radius);
    }

    Vector2 FindCenterPoint(Camera refCam)
    {
        Vector2 screenTopRight = refCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBottomLeft = refCam.ScreenToWorldPoint(new Vector2(0, 0));
        float bottomMostY = screenBottomLeft.y + maxRadius;
        float topMostY = screenTopRight.y - maxRadius;
        float leftMostX = screenBottomLeft.x + maxRadius;
        float rightMostX = screenTopRight.x - maxRadius;
        return new Vector2(Random.Range(leftMostX, rightMostX), Random.Range(bottomMostY, topMostY));
    }
}
