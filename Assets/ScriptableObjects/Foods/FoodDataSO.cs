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
public enum GGLReason
{
    None,
    Sugar,
    Salt,
    Fat,
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

    public AttributeField(FoodAttributeFieldId fieldId, string fieldName, string value)
    {
        FieldId = fieldId;
        FieldName = fieldName;
        Value = value;
    }
}
[System.Serializable]
public class FoodComponents
{
    [Tooltip("Type of component, e.g., 'Tabel Nutrisi', 'Komposisi', 'Sajian'")]
    public FoodComponentsType ComponentType; // "Tabel Nutrisi", "Komposisi", "Sajian"

    [Tooltip("List of attribute fields for this component (e.g., 'sugar, Glucose, 20g', 'salt, Sodium, 15mg')")]
    public List<AttributeField> AttributeFields;

    public FoodComponents(FoodComponentsType componentType, List<string> values)
    {
        ComponentType = componentType;

                // Values should be in csv
        switch (componentType)
        {
            case FoodComponentsType.Serving:
                AttributeFields = new List<AttributeField>()
                {
                    new(FoodAttributeFieldId.ServingSize, "Takaran Saji", values[0]),
                    new(FoodAttributeFieldId.ServingCount, "Sajian per Kemasan", values[1]),
                    new(FoodAttributeFieldId.Calories, "Kalori per Sajian", values[2])
                };
                break;
            case FoodComponentsType.Nutrition:
                AttributeFields = new List<AttributeField>()
                {
                    new(FoodAttributeFieldId.TotalFat, "Lemak Total", values[0]),
                    new(FoodAttributeFieldId.SaturatedFat, "Lemak Jenuh", values[1]),
                    new(FoodAttributeFieldId.Protein, "Protein", values[2]),
                    new(FoodAttributeFieldId.Carbohydrates, "Karbohidrat", values[3]),
                    new(FoodAttributeFieldId.Sugar, "Gula", values[4]),
                    new(FoodAttributeFieldId.Sodium, "Natrium", values[5])
                };
                break;
            case FoodComponentsType.Composition:
                AttributeFields = new List<AttributeField>()
                {
                    new(FoodAttributeFieldId.Composition, "Komposisi", values[0]),
                };
                break;
        }
    }
}
[CreateAssetMenu(fileName = "NewFoodData", menuName = "ScriptableObjects/FoodData", order = 1)]
public class FoodDataSO : ScriptableObject
{
    public string FoodId;
    public GameObject FoodPrefab;
    public List<FoodComponents> Components = new List<FoodComponents>();

    [Header("Actual Results")]
    public ApprovalResult Approval;
    public GGLSticker GGLRating;
    public List<GGLReason> GGLReasons = new List<GGLReason>();
    public bool IsKadaluarsa;
    public bool IsDefect;
    public List<Claim> Claims;

    public void SetComponents(List<string> values)
    {
        Components.Clear();
        // Assuming values are provided in the correct order for each component type
        Components.Add(new FoodComponents(FoodComponentsType.Serving, values.GetRange(0, 3)));
        Components.Add(new FoodComponents(FoodComponentsType.Nutrition, values.GetRange(3, 6)));
        Components.Add(new FoodComponents(FoodComponentsType.Composition, values.GetRange(9, 1)));

        GGLRating = values[9] switch
        {
            "A" => GGLSticker.A,
            "B" => GGLSticker.B,
            "C" => GGLSticker.C,
            "D" => GGLSticker.D,
            _ => GGLSticker.A,
        };
    }
}