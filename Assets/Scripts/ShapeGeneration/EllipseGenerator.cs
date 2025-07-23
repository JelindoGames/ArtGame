using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Generator/Ellipse")]
public class EllipseGenerator : ShapeGenerator
{
    [SerializeField] float minSizeX;
    [SerializeField] float maxSizeX;
    [SerializeField] float minSizeY;
    [SerializeField] float maxSizeY;

    public override IShape Generate(Camera referenceCam)
    {
        Vector2 centerPoint = FindCenterPoint(referenceCam);
        float xSize = Random.Range(minSizeX, maxSizeX);
        float ySize = Random.Range(minSizeY, maxSizeY);
        float angle = Random.Range(0, Mathf.PI * 2);
        return new EllipseShape(centerPoint, xSize, ySize, angle);
    }

    Vector2 FindCenterPoint(Camera refCam)
    {
        Vector2 screenTopRight = refCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBottomLeft = refCam.ScreenToWorldPoint(new Vector2(0, 0));
        float maxSizeGeneral = Mathf.Max(maxSizeX, maxSizeY);
        float bottomMostY = screenBottomLeft.y + maxSizeGeneral;
        float topMostY = screenTopRight.y - maxSizeGeneral;
        float leftMostX = screenBottomLeft.x + maxSizeGeneral;
        float rightMostX = screenTopRight.x - maxSizeGeneral;
        return new Vector2(Random.Range(leftMostX, rightMostX), Random.Range(bottomMostY, topMostY));
    }
}
