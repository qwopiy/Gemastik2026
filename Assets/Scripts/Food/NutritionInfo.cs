using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NutritionInfo : MonoBehaviour
{
    [Header("Sajian Elements")]
    public TextMeshProUGUI portionText;
    public TextMeshProUGUI portionPerPcsText;
    public TextMeshProUGUI caloriesText;
    [Header("Tabel Nutrisi Elements")]
    public TextMeshProUGUI totalFatText;
    public TextMeshProUGUI saturatedFatText;
    public TextMeshProUGUI proteinText;
    public TextMeshProUGUI totalCarbohydratesText;
    public TextMeshProUGUI sugarText;
    public TextMeshProUGUI sodiumText;

    [Header("Komposisi Settings")]
    public TextMeshProUGUI compositionText;
    private string composition = "<b>Komposisi</b>";
    public void SetNutrition(List<FoodComponents> list)
    {
        foreach (var item in list)
        {
            switch (item)
            {
                case FoodComponents components when components.ComponentType == FoodComponentsType.Serving:
                    foreach (var attribute in components.AttributeFields)
                    {
                        switch (attribute.FieldId)
                        {
                            case FoodAttributeFieldId.ServingSize:
                                portionText.text = $"Takaran Saji {attribute.Value}";
                                break;
                            case FoodAttributeFieldId.ServingCount:
                                portionPerPcsText.text = $"{attribute.Value} Sajian per Kemasan";
                                break;
                            case FoodAttributeFieldId.Calories:
                                caloriesText.text = $"{attribute.Value} kkal";
                                break;
                        }
                    }
                    break;

                case FoodComponents components when components.ComponentType == FoodComponentsType.Nutrition:
                    foreach (var attribute in components.AttributeFields)
                    {
                        switch (attribute.FieldId)
                        {
                            case FoodAttributeFieldId.TotalFat:
                                totalFatText.text = $"{attribute.Value}g";
                                break;
                            case FoodAttributeFieldId.SaturatedFat:
                                saturatedFatText.text = $"{attribute.Value}g";
                                break;
                            case FoodAttributeFieldId.Protein:
                                proteinText.text = $"{attribute.Value}g";
                                break;
                            case FoodAttributeFieldId.Carbohydrates:
                                totalCarbohydratesText.text = $"{attribute.Value}g";
                                break;
                            case FoodAttributeFieldId.Sugar:
                                sugarText.text = $"{attribute.Value}g";
                                break;
                            case FoodAttributeFieldId.Sodium:
                                sodiumText.text = $"{attribute.Value}mg";
                                break;
                        }
                    }
                    break;

                case FoodComponents components when components.ComponentType == FoodComponentsType.Composition:
                    foreach (var attribute in components.AttributeFields)
                    {
                        if (attribute.FieldId == FoodAttributeFieldId.Composition)
                        {
                            composition = attribute.Value;
                            compositionText.text = $"<b>Komposisi</b>\n{composition}";
                        }
                    }
                    break;

                default:
                    Debug.LogWarning($"Unknown component type: {item.ComponentType}");
                    break;
            }
        }
    }
}
