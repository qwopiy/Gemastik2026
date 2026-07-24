using UnityEngine;
using System.Collections.Generic;
public enum FoodComponentsType
{
    Serving,
    Nutrition,
    Composition,
}

public enum FoodAttributeFieldId
{
    ServingSize,
    ServingCount,
    Calories,
    TotalFat,
    SaturatedFat,
    Protein,
    Carbohydrates,
    Sugar,
    Sodium,
    Composition
}

// Individual attribute field that the player can inspect/click
[System.Serializable]
public class AttributeField
{
    [Tooltip("list of id in nutrition = total_fat, saturated_fat, carbohydrates, sugar, sodium, protein" +
        "list of id in sajian = serving_count, calories" +
        "list of id in komposisi = komposisi")]
    public FoodAttributeFieldId FieldId;    // e.g., "sugar", "salt", "fat", "expiry_date"
    public string FieldName;    // e.g., "Glucose", "Sodium", "Trans Fat", "Expiry Date"
    public string Value;      // e.g., "26g", "15mg", "03/12/2026"
}
[System.Serializable]
public class FoodComponents
{
    [Tooltip("Type of component, e.g., 'Tabel Nutrisi', 'Komposisi', 'Sajian'")]
    public FoodComponentsType ComponentType; // "Tabel Nutrisi", "Komposisi", "Sajian"

    [Tooltip("List of attribute fields for this component (e.g., 'sugar, Glucose, 20g', 'salt, Sodium, 15mg')")]
    public List<AttributeField> AttributeFields;
}
[CreateAssetMenu(fileName = "NewFoodData", menuName = "ScriptableObjects/FoodData", order = 1)]
public class FoodData : ScriptableObject
{
    public string FoodId;
    public GameObject FoodPrefab;
    public List<FoodComponents> Components = new List<FoodComponents>();

    // Ground Truth set by the generator (hidden from player)
    public bool ShouldBeApproved;
    public string InvalidReason; // e.g., "Expired Food", "Sugar Too High"

    public void SpawnFood(Transform parent)
    {
        if (FoodPrefab != null)
        {
            GameObject foodInstance = Instantiate(FoodPrefab, parent);
            foodInstance.GetComponent<FoodInfo>().SetNutrition(Components);
            foodInstance.name = FoodId;
        }
        else
        {
            Debug.LogWarning($"FoodPrefab is not assigned for FoodData: {FoodId}");
        }
    }
}