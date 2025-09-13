using UnityEngine;

public class Stone : MonoBehaviour
{
    [Header("Stone Colors")]
    public Color blackColor = Color.black;
    public Color whiteColor = Color.white;
    
    private SpriteRenderer spriteRenderer;
    private bool isBlack;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        CreateStoneSprite();
    }
    
    void CreateStoneSprite()
    {
        Texture2D texture = new Texture2D(64, 64);
        Vector2 center = new Vector2(32, 32);
        float radius = 30f;
        
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }
        
        texture.Apply();
        
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 100);
        spriteRenderer.sprite = sprite;
    }
    
    public void SetStone(bool black)
    {
        isBlack = black;
        spriteRenderer.color = isBlack ? blackColor : whiteColor;
        
        if (!isBlack)
        {
            GameObject outline = new GameObject("Outline");
            outline.transform.SetParent(transform);
            outline.transform.localPosition = Vector3.zero;
            outline.transform.localScale = Vector3.one * 1.1f;
            
            SpriteRenderer outlineRenderer = outline.AddComponent<SpriteRenderer>();
            outlineRenderer.sprite = spriteRenderer.sprite;
            outlineRenderer.color = Color.black;
            outlineRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        }
    }
    
    public bool IsBlack()
    {
        return isBlack;
    }
}