using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FoodItem : MonoBehaviour
{
    // data tabel nutrisi
    public GameObject textInNutritionPrefab;
    public List<string> nutritionData;
    public Transform fatParent;
    public Transform carbsParent;
    public Transform sugarParent;
    public Transform sodiumParent;

    // data sajian
    public int servingCount;
    public TextMeshProUGUI servingCountText;
    public int calories;
    public TextMeshProUGUI caloriesText;

    // data komposisi
    public List<string> compositionData;

    public void InitializeNutrition(FoodComponents nutrition)
    {
        foreach (var attribute in nutrition.AttributeFields)
        {
            GameObject textObj = Instantiate(textInNutritionPrefab);
            textObj.transform.SetParent(GetParentForField(attribute.FieldId), false);
            TextMeshProUGUI textComponent = textObj.GetComponent<TextMeshProUGUI>();
            textComponent.text = $"{attribute.FieldName}: {attribute.Value}";
        }
    }

    private Transform GetParentForField(FoodAttributeFieldId fieldId)
    {
        return fieldId switch
        {
            FoodAttributeFieldId.TotalFat => fatParent,
            FoodAttributeFieldId.Carbohydrates => carbsParent,
            FoodAttributeFieldId.Sugar => sugarParent,
            FoodAttributeFieldId.Sodium => sodiumParent,
            _ => throw new ArgumentException("Unknown field ID"),
        };
    }
}
