using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script for a Sprite that can be clicked on to draw.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Paper : MonoBehaviour
{
    [SerializeField] int brushSize;

    Camera referenceCamera;
    SpriteRenderer spriteRenderer;
    Texture2D tex;
    Color32[] brushStroke;

    InputMap input;
    bool inputPressing;
    Vector2 inputCursorPosition;
    Vector2Int? lastFrameCursorPosition;

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
    }

    void OnPositionChange(InputAction.CallbackContext ctx)
    {
        inputCursorPosition = ctx.ReadValue<Vector2>();
    }

    void Start()
    {
        referenceCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        Texture2D sharedTex = spriteRenderer.sprite.texture;

        // Save the brush stroke here to it doesn't have to be dealt with again
        brushStroke = new Color32[brushSize * brushSize];
        Color32 black = new(0, 0, 0, 255);
        for (int i = 0; i < brushSize * brushSize; i++)
        {
            brushStroke[i] = black;
        }

        tex = Instantiate(sharedTex); // Give each instance a unique texture
    }

    void Update()
    {
        if (inputPressing)
        {
            Vector2Int drawingPosition = TexturePositionOfCursor();
            if (TexturePositionIsValid(drawingPosition))
            {
                DrawUpTo(drawingPosition);
                lastFrameCursorPosition = drawingPosition;
                return;
            }
        }
        lastFrameCursorPosition = null;
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

        tex.Apply();
        // TODO for some reason the PPU needs to be the same as the texture size for the full texture to be drawable
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 614);
        spriteRenderer.sprite = newSprite;
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
        Vector3 mousePos = inputCursorPosition;
        Vector3 worldPos = referenceCamera.ScreenToWorldPoint(mousePos);
        Vector3 localPos = transform.InverseTransformPoint(worldPos);

        Sprite sprite = spriteRenderer.sprite;
        Rect rect = sprite.textureRect;
        Vector2 pivot = sprite.pivot;
        float pixelsPerUnit = sprite.pixelsPerUnit;

        // The position "within" the sprite rect
        float localX = pivot.x + (localPos.x * pixelsPerUnit);
        float localY = pivot.y + (localPos.y * pixelsPerUnit);

        int texX = (int)((localX / rect.width) * tex.width);
        int texY = (int)((localY / rect.height) * tex.height);

        return new Vector2Int(texX, texY);
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
}
