using UnityEngine;

[CreateAssetMenu(fileName = "NewFoodDisplay", menuName = "ScriptableObjects/FoodDisplay")]
public class FoodDisplaySO : ScriptableObject
{
    [Header("Text")]
    public string itemName;
    public string ExpiredDate;
    public string BrandClaim;

    [Header("Sprites")]
    public Sprite baseSprite;
    public Sprite maskSprite;

    [Header("Overlays (Optional)")]
    public Sprite brandSprite;
    public Sprite conditionSprite;

    [Header("Palette Swap Colors")]
    public Color primaryColor = Color.red;
    public Color secondaryColor = Color.green;
    public Color tertiaryColor = Color.blue;
}
