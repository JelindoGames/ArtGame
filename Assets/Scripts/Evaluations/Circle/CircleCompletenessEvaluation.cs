using UnityEngine;

/// <summary>
/// Evaluates how complete a raster circle is.
/// </summary>
[CreateAssetMenu(menuName = "Evaluation/Circle/Circle Completeness")]
public class CircleCompletenessEvaluation : CircleEvaluation
{
    [SerializeField] float scalingFactor;

    public override float Compare(CircleShape vector, PenStroke raster)
    {
        float incompleteScore = 0; // The less of the circle the stroke covers, the higher the incomplete score is
        for (float theta = 0; theta < Mathf.PI * 2 - Mathf.Epsilon; theta += Mathf.PI / 4)
        {
            Vector2 pointAtAngle = new(vector.Center.x + (Mathf.Cos(theta) * vector.Radius), vector.Center.y + (Mathf.Sin(theta) * vector.Radius));
            Vector2 closestPointOnStroke = raster.ClosestPointOnStrokeTo(pointAtAngle);
            float distance = Vector2.Distance(closestPointOnStroke, pointAtAngle);
            incompleteScore += distance;
        }
        incompleteScore /= 8; // Average out the points we looked at

        return Mathf.Clamp(incompleteScore * scalingFactor, 0, 1);
    }
}
