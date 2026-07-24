using UnityEngine;
using System.Collections.Generic;

// Individual attribute field that the player can inspect/click
[System.Serializable]
public class AttributeField
{
    [Tooltip("list of id in nutrition = fat, carbs, sugar, sodium" +
        "list of id in sajian = serving_count, calories" +
        "list of id in komposisi = komposisi")]
    public string FieldId;    // e.g., "sugar", "salt", "fat", "expiry_date"
    public string FieldName;    // e.g., "Glucose", "Sodium", "Trans Fat", "Expiry Date"
    public string Value;      // e.g., "26g", "15mg", "03/12/2026"
}
[System.Serializable]
public class FoodComponents
{
    [Tooltip("Type of component, e.g., 'Tabel Nutrisi', 'Komposisi', 'Sajian'")]
    public string ComponentType; // "Tabel Nutrisi", "Komposisi", "Sajian"

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


}