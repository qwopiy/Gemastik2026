using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class NutritionText : MonoBehaviour
{
    public TextMeshProUGUI label;
    public TextMeshProUGUI value;
    public TextMeshProUGUI AKG;
    private float akgValue; // TODO: Set this value based on the specific nutrient's AKG

    public void SetNutrition(string labelText, string valueText)
    {
        label.text = labelText;
        value.text = valueText;
        
        AKG.text = $"({GetAKGPercentage(valueText)}%)";
    }

    public float GetAKGPercentage(string valueText)
    {
        float value = float.Parse(Regex.Replace(valueText, @"[^\d.]", ""));
        return GetAKGPercentage(value);
    }
    public float GetAKGPercentage(float value)
    {
        return value / akgValue * 100;
    }
}