using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NutritionInfo : MonoBehaviour
{
    [Header("Sajian Elements")]
    public TextMeshProUGUI portionText;
    private int portionSize = 1;
    public TextMeshProUGUI portionPerPcsText;
    private int portionPerPcs = 1;
    public TextMeshProUGUI caloriesText;
    private int totalCalories = 1;
    [Header("Tabel Nutrisi Elements")]
    public TextMeshProUGUI totalFatText;
    private int totalFat = 1;
    public TextMeshProUGUI saturatedFatText;
    private int saturatedFat = 1;
    public TextMeshProUGUI proteinText;
    private int protein = 1;
    public TextMeshProUGUI totalCarbohydratesText;
    private int totalCarbohydrates = 1;
    public TextMeshProUGUI sugarText;
    private int sugar = 1;
    public TextMeshProUGUI sodiumText;
    private int sodium = 1;

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
                                portionSize = int.Parse(attribute.Value);
                                portionText.text = $"Takaran Saji {portionSize}g";
                                break;
                            case FoodAttributeFieldId.ServingCount:
                                portionPerPcs = int.Parse(attribute.Value);
                                portionPerPcsText.text = $"{portionPerPcs} Sajian per Kemasan";
                                break;
                            case FoodAttributeFieldId.Calories:
                                totalCalories = int.Parse(attribute.Value);
                                caloriesText.text = $"{totalCalories} kkal";
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
                                totalFat = int.Parse(attribute.Value);
                                totalFatText.text = $"{totalFat}g";
                                Debug.Log("Total Fat: " + totalFat);
                                break;
                            case FoodAttributeFieldId.SaturatedFat:
                                saturatedFat = int.Parse(attribute.Value);
                                saturatedFatText.text = $"{saturatedFat}g";
                                break;
                            case FoodAttributeFieldId.Protein:
                                protein = int.Parse(attribute.Value);
                                proteinText.text = $"{protein}g";
                                break;
                            case FoodAttributeFieldId.Carbohydrates:
                                totalCarbohydrates = int.Parse(attribute.Value);
                                totalCarbohydratesText.text = $"{totalCarbohydrates}g";
                                break;
                            case FoodAttributeFieldId.Sugar:
                                sugar = int.Parse(attribute.Value);
                                sugarText.text = $"{sugar}g";
                                break;
                            case FoodAttributeFieldId.Sodium:
                                sodium = int.Parse(attribute.Value);
                                sodiumText.text = $"{sodium}mg";
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
