using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script for a Sprite that can be clicked on to draw.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class Paper : MonoBehaviour
{
    [SerializeField] int brushSize;
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] StrokeEvent onStrokeCompleted;

    RectTransform rectTransform;
    Camera referenceCamera;
    Color32[] initialTexColoring;
    Texture2D tex;
    Color32[] brushStroke;

    InputMap input;
    bool inputPressing;
    Vector2 inputCursorPosition;
    Vector2Int? lastFrameCursorPosition;

    List<Vector2> positionsInCurrentStroke = new();

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

    private void OnDisable()
    {
        input.Disable();
    }

    void OnPress(InputAction.CallbackContext ctx)
    {
        inputPressing = true;
    }

    void OnRelease(InputAction.CallbackContext ctx)
    {
        inputPressing = false;
        lastFrameCursorPosition = null;
        PenStroke finishedStroke = new(positionsInCurrentStroke);
        onStrokeCompleted.Raise(finishedStroke);
        positionsInCurrentStroke = new List<Vector2>();
        Clear();
    }

    void OnPositionChange(InputAction.CallbackContext ctx)
    {
        inputCursorPosition = ctx.ReadValue<Vector2>();
        if (inputPressing)
        {
            Vector2Int drawingPosition = TexturePositionOfCursor();
            if (TexturePositionIsValid(drawingPosition))
            {
                positionsInCurrentStroke.Add(WorldPositionOfCursor());
                DrawUpTo(drawingPosition);
                lastFrameCursorPosition = drawingPosition;
            }
        }
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        referenceCamera = Camera.main;

        // Create the texture and color it white
        tex = new Texture2D(2048, 2048);
        initialTexColoring = new Color32[tex.width * tex.height];
        Color32 white = new(255, 255, 255, 255);
        for (long i = 0; i < tex.width * tex.height; i++)
        {
            initialTexColoring[i] = white;
        }
        tex.SetPixels32(initialTexColoring);
        ApplyTextureChanges();

        // Save the brush stroke here to it doesn't have to be dealt with again
        brushStroke = new Color32[brushSize * brushSize];
        Color32 black = new(0, 0, 0, 255);
        for (int i = 0; i < brushSize * brushSize; i++)
        {
            brushStroke[i] = black;
        }
    }

    /// <summary>
    /// Draw from the last frame's drawing position (if it exists) to the given drawing position.
    /// Includes smoothing (extra pixels in the middle).
    /// Applies all texture changes.
    /// </summary>
    void DrawUpTo(Vector2Int drawingPosition)
    {
        // If we need smoothing from last frame to this one
        if (lastFrameCursorPosition.HasValue)
        {
            int smoothingPixelsNeeded = (int)Vector2Int.Distance(lastFrameCursorPosition.Value, drawingPosition);
            for (int i = 1; i < smoothingPixelsNeeded; i++)
            {
                Vector2 midpoint = Vector2.Lerp(lastFrameCursorPosition.Value, drawingPosition, (float)i / smoothingPixelsNeeded);
                DrawAt(new Vector2Int((int)midpoint.x, (int)midpoint.y));
            }
        }
        DrawAt(drawingPosition);
        ApplyTextureChanges();
    }

    /// <summary>
    /// Draw at the given exact position.
    /// Does not apply any texture changes.
    /// </summary>
    void DrawAt(Vector2Int drawingPosition)
    {
        int actualBrushSizeX = Mathf.Min(brushSize, tex.width - drawingPosition.x);
        int actualBrushSizeY = Mathf.Min(brushSize, tex.height - drawingPosition.y);
        tex.SetPixels32(drawingPosition.x, drawingPosition.y, actualBrushSizeX, actualBrushSizeY, brushStroke);
    }

    /// <summary>
    /// Translate the current cursor position to a position on the paper texture.
    /// </summary>
    Vector2Int TexturePositionOfCursor()
    {
        Vector2 cursorPos = inputCursorPosition;
        Vector2 bottomLeftScreenPoint = referenceCamera.WorldToScreenPoint(rectTransform.rect.min);

        // (0, 0) is bottom-left of the rect
        Vector2 relativeCursorPos = new(cursorPos.x - bottomLeftScreenPoint.x, cursorPos.y - bottomLeftScreenPoint.y);
        // No matter what resolution, the screen height (on ortho size 5) always seems to be 10 world units tall
        Vector2 relativeCursorPosScaled = new(relativeCursorPos.x * 10 / rectTransform.rect.width, relativeCursorPos.y * 10 / rectTransform.rect.height);
        relativeCursorPosScaled *= referenceCamera.orthographicSize / 5;
        relativeCursorPosScaled *= (float)tex.height / (float)Screen.height;

        return new Vector2Int((int)relativeCursorPosScaled.x, (int)relativeCursorPosScaled.y);
    }

    /// <summary>
    /// Translate the current cursor position to a world position.
    /// </summary>
    Vector3 WorldPositionOfCursor()
    {
        return referenceCamera.ScreenToWorldPoint(inputCursorPosition);
    }

    /// <summary>
    /// Is the given texture position actually on the paper's texture?
    /// </summary>
    bool TexturePositionIsValid(Vector2Int texturePosition)
    {
        return texturePosition.x >= 0 &&
            texturePosition.y >= 0 &&
            texturePosition.x < tex.width &&
            texturePosition.y < tex.height;
    }

    /// <summary>
    /// Clears what has been drawn so far.
    /// </summary>
    void Clear()
    {
        tex.SetPixels32(initialTexColoring);
        ApplyTextureChanges();
    }

    /// <summary>
    /// Takes the changes to the working texture, and actually visualizes them with a new sprite.
    /// </summary>
    void ApplyTextureChanges()
    {
        tex.Apply();
        Graphics.Blit(tex, renderTexture);
    }
}
