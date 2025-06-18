using UnityEngine;

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
    //Color32[] originalColoring;
    Texture2D tex;

    bool mouseOver;
    Vector2Int? lastFrameMousePosition;

    Color32[] brushStroke;

    /*
    InputMap input;

    void Awake()
    {
        input = new InputMap();
        input.Drawing.Position.performed += OnPositionChange;
        input.Drawing.Press.performed += OnPress;
        input.Drawing.Press.canceled += OnRelease;
    }
    */

    void Start()
    {
        referenceCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        Texture2D sharedTex = spriteRenderer.sprite.texture;
        //originalColoring = sharedTex.GetPixels32();

        // Save the brush stroke here to it doesn't have to be dealt with again
        brushStroke = new Color32[brushSize * brushSize];
        Color32 black = new(0, 0, 0, 255);
        for (int i = 0; i < brushSize * brushSize; i++)
        {
            brushStroke[i] = black;
        }

        tex = Instantiate(sharedTex); // Give each instance a unique texture
    }

    void OnMouseEnter()
    {
        mouseOver = true;
    }

    void OnMouseExit()
    {
        mouseOver = false;
    }

    void Update()
    {
        if (mouseOver && Input.GetMouseButton(0))
        {
            Vector2Int cleaningPosition = TexturePositionOfMouse();
            DrawUpTo(cleaningPosition);
            lastFrameMousePosition = cleaningPosition;
        }
        else
        {
            lastFrameMousePosition = null;
        }
    }

    /// <summary>
    /// Draw from the last frame's cleaning position (if it exists) to the given cleaning position.
    /// Includes smoothing (extra pixels in the middle).
    /// </summary>
    void DrawUpTo(Vector2Int cleaningPosition)
    {
        // If we need smoothing from last frame to this one
        if (lastFrameMousePosition.HasValue)
        {
            int smoothingPixelsNeeded = (int)Vector2Int.Distance(lastFrameMousePosition.Value, cleaningPosition);
            for (int i = 1; i < smoothingPixelsNeeded; i++)
            {
                Vector2 midpoint = Vector2.Lerp(lastFrameMousePosition.Value, cleaningPosition, (float)i / smoothingPixelsNeeded);
                DrawAt(new Vector2Int((int)midpoint.x, (int)midpoint.y));
            }
        }
        DrawAt(cleaningPosition);

        tex.Apply();
        // TODO for some reason the PPU needs to be the same as the texture size for the full texture to be paintable
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 614);
        spriteRenderer.sprite = newSprite;
    }

    /// <summary>
    /// Draw at the given exact position.
    /// </summary>
    void DrawAt(Vector2Int cleaningPosition)
    {
        int actualBrushSizeX = Mathf.Min(brushSize, tex.width - cleaningPosition.x);
        int actualBrushSizeY = Mathf.Min(brushSize, tex.height - cleaningPosition.y);
        tex.SetPixels32(cleaningPosition.x, cleaningPosition.y, actualBrushSizeX, actualBrushSizeY, brushStroke);
    }

    Vector2Int TexturePositionOfMouse()
    {
        Vector3 mousePos = Input.mousePosition;
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

    /*
    void OnDestroy()
    {
        tex.SetPixels32(originalColoring);
        tex.Apply();
    }
    */
}
