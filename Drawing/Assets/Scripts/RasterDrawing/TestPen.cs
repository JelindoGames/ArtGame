using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPen : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform paper;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] float gapFillStepSize;
    [SerializeField] StrokeEvent onStroke;
    InputMap input;
    bool pressing;

    List<Vector2> pointsInCurrentStroke = new List<Vector2>();

    void Awake()
    {
        input = new InputMap();
        input.Drawing.Position.performed += OnPositionChange;
        input.Drawing.Press.performed += OnPress;
        input.Drawing.Press.canceled += OnRelease;
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void OnPress(InputAction.CallbackContext ctx)
    {
        pressing = true;
    }

    void OnRelease(InputAction.CallbackContext ctx)
    {
        pressing = false;
        PenStroke finishedStroke = new PenStroke(pointsInCurrentStroke);
        onStroke.Raise(finishedStroke);
        pointsInCurrentStroke = new List<Vector2>();
    }

    void OnPositionChange(InputAction.CallbackContext ctx)
    {
        Vector2 pixelPos = ctx.ReadValue<Vector2>();
        Vector2 worldPos = cam.ScreenToWorldPoint(pixelPos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0);
        if (pressing)
        {
            // Fill the area that wasn't actually "covered" during the frame
            if (pointsInCurrentStroke.Count > 0)
            {
                Vector2 lastDot = pointsInCurrentStroke[pointsInCurrentStroke.Count - 1];
                List<Vector2> dotsToAdd = InBetweenPositions(lastDot, transform.position);
                foreach (Vector2 pos in dotsToAdd)
                {
                    GameObject fillerDot = Instantiate(dotPrefab, paper);
                    fillerDot.transform.position = pos;
                }
            }
            GameObject dot = Instantiate(dotPrefab, paper);
            dot.transform.position = transform.position;
            pointsInCurrentStroke.Add(transform.position);
        }
    }

    List<Vector2> InBetweenPositions(Vector2 p1, Vector2 p2)
    {
        List<Vector2> inBetweenPositions = new List<Vector2>();
        Vector2 p1Screen = cam.WorldToScreenPoint(p1);
        Vector2 p2Screen = cam.WorldToScreenPoint(p2);
        int steps = (int)(Vector2.Distance(p1Screen, p2Screen) / gapFillStepSize);
        for (int step = 1; step < steps - 1; step++)
        {
            inBetweenPositions.Add(Vector2.Lerp(p1, p2, (float)step / steps));
        }
        return inBetweenPositions;
    }
}
