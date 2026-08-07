using UnityEngine;

[CreateAssetMenu(fileName = "NewFoodDisplay", menuName = "ScriptableObjects/FoodDisplay")]
public class FoodDisplaySO : ScriptableObject
{
    public string itemName;

    [Header("Sprites")]
    public Sprite baseSprite;
    public Sprite maskSprite;

    [Header("Overlays (Opsional)")]
    public Sprite brandSprite;
    public Sprite conditionSprite;

    [Header("Palette Swap Colors")]
    public Color primaryColor = Color.red;
    public Color secondaryColor = Color.green;
    public Color tertiaryColor = Color.blue;
}
