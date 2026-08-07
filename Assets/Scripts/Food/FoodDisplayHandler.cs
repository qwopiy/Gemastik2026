using UnityEngine;

public class FoodDisplayHandler : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private FoodDisplaySO itemData;

    [Header("Child Renderers")]
    [SerializeField] private SpriteRenderer brandRenderer;
    [SerializeField] private SpriteRenderer conditionRenderer;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;

    private static readonly int PrimaryColorID = Shader.PropertyToID("_PrimaryColor");
    private static readonly int SecondaryColorID = Shader.PropertyToID("_SecondaryColor");
    private static readonly int TertiaryColorID = Shader.PropertyToID("_TertiaryColor");
    private static readonly int MaskTexID = Shader.PropertyToID("_MaskTex");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        if (itemData != null)
        {
            ApplyItemData(itemData);
        }
    }

    public void ApplyItemData(FoodDisplaySO newItemData)
    {
        itemData = newItemData;

        if (itemData == null) return;

        spriteRenderer.sprite = itemData.baseSprite;

        spriteRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetColor(PrimaryColorID, itemData.primaryColor);
        propertyBlock.SetColor(SecondaryColorID, itemData.secondaryColor);
        propertyBlock.SetColor(TertiaryColorID, itemData.tertiaryColor);

        if (itemData.maskSprite != null)
        {
            propertyBlock.SetTexture(MaskTexID, itemData.maskSprite.texture);
        }

        spriteRenderer.SetPropertyBlock(propertyBlock);

        if (brandRenderer != null)
        {
            brandRenderer.sprite = itemData.brandSprite;
            brandRenderer.gameObject.SetActive(itemData.brandSprite != null);
        }

        if (conditionRenderer != null)
        {
            conditionRenderer.sprite = itemData.conditionSprite;
            conditionRenderer.gameObject.SetActive(itemData.conditionSprite != null);
        }
    }
}
