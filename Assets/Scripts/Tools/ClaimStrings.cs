using System.Collections.Generic;

public struct ClaimStrings
{
    public ClaimType type;
    public List<string> descriptions;

    public ClaimStrings(ClaimType claimType, List<string> claimDescriptions)
    {
        type = claimType;
        descriptions = claimDescriptions;
    }

    public string GetRandomDescription()
    {
        if (descriptions.Count == 0)
        {
            return string.Empty;
        }
        int randomIndex = UnityEngine.Random.Range(0, descriptions.Count);
        return descriptions[randomIndex];
    }
}

public static class ClaimStringsDatabase
{
    public static List<ClaimStrings> claimStringsList = new()
    {
        new(ClaimType.None, new List<string> { "Tidak ada klaim", "No claims", "No specific claims" }),
        new(ClaimType.CalorieFree, new List<string> { "Tanpa Kalori", "Calorie-free", "Zero calories" }),
        new(ClaimType.LowTotalFat, new List<string> { "Rendah Lemak", "Low in fat", "Reduced fat content" }),
        new(ClaimType.HighProtein, new List<string> { "Tinggi Protein", "High in protein", "Protein-rich", "Cocok untuk olahraga", "Pembangun Otot!" }),
        new(ClaimType.LowCarbohydrate, new List<string> { "Rendah Karbohidrat", "Low in carbohydrates", "Reduced carb content" }),
        new(ClaimType.LowSugar, new List<string> { "Rendah Gula", "Low in sugar", "Reduced sugar content" }),
        new(ClaimType.LowSalt, new List<string> { "Rendah Garam", "Low in salt", "Reduced salt content" }),
        new(ClaimType.Healthy, new List<string> { "Sehat", "Sehat untuk badan", "Good for your health" }),
        new(ClaimType.NoPreservative, new List<string> { "Tanpa Pengawet", "No Preservatives", "Preservative-free" }),
        new(ClaimType.NutriLevel, new List<string> { "Nutri-Level A", "Nutri-Level B", "Nutri-Level C", "Nutri-Level D" }),
        new(ClaimType.Composition, new List<string> { "" }),
    };
    public static string GetRandomDescription(ClaimType claimType)
    {
        foreach (var claimStrings in claimStringsList)
        {
            if (claimStrings.type == claimType)
            {
                return claimStrings.GetRandomDescription();
            }
        }
        return string.Empty; // Return empty if no matching claim type is found
    }
}