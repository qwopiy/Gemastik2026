using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class NutritionValue : MonoBehaviour
{
    public TextMeshProUGUI TMPObj;
    public float value;
    public TextMeshProUGUI AKG;
    public float akgValue; // TODO: Set this value based on the specific nutrient's AKG

    public void SetNutrition()
    {
        TMPObj.text = $"{value}";
        
        AKG.text = $"({GetAKGPercentage(value)}%)";
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