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

    [Header("AKG Elements")]
    public TextMeshProUGUI totalFatAKGText;
    public TextMeshProUGUI saturatedFatAKGText;
    public TextMeshProUGUI proteinAKGText;
    public TextMeshProUGUI totalCarbohydratesAKGText;
    //public TextMeshProUGUI sugarAKGText;
    public TextMeshProUGUI sodiumAKGText;

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

                        // Set AKG values
                        string akgValue = GetAKGValue(attribute);
                        switch (attribute.FieldId)
                        {
                            case FoodAttributeFieldId.TotalFat:
                                totalFatAKGText.text = akgValue;
                                break;
                            case FoodAttributeFieldId.SaturatedFat:
                                saturatedFatAKGText.text = akgValue;
                                break;
                            case FoodAttributeFieldId.Protein:
                                proteinAKGText.text = akgValue;
                                break;
                            case FoodAttributeFieldId.Carbohydrates:
                                totalCarbohydratesAKGText.text = akgValue;
                                break;
                            case FoodAttributeFieldId.Sodium:
                                sodiumAKGText.text = akgValue;
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

    private string GetAKGValue(AttributeField attribute)
    {
        float akg = 0f;
        switch (attribute.FieldId)
        {
            case FoodAttributeFieldId.TotalFat:
                akg = float.TryParse(attribute.Value, out float totalFat) ? (totalFat / 67f) * 100f : 0f;
                return $"{akg:F0}%";
            case FoodAttributeFieldId.SaturatedFat:
                akg = float.TryParse(attribute.Value, out float saturatedFat) ? (saturatedFat / 20f) * 100f : 0f;
                return $"{akg:F0}%";
            case FoodAttributeFieldId.Protein:
                akg = float.TryParse(attribute.Value, out float protein) ? (protein / 60f) * 100f : 0f;
                return $"{akg:F0}%";
            case FoodAttributeFieldId.Carbohydrates:
                akg = float.TryParse(attribute.Value, out float carbohydrates) ? (carbohydrates / 340f) * 100f : 0f;
                return $"{akg:F0}%";
            case FoodAttributeFieldId.Sodium:
                akg = float.TryParse(attribute.Value, out float sodium) ? (sodium / 2000f) * 100f : 0f;
                return $"{akg:F0}%";
        }
        return string.Empty;
    }
}
