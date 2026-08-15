using System.Collections.Generic;
using UnityEngine;

public enum ClaimType
{
    None,
    CalorieFree, // < 5 calories per serving
    HighProtein, // > 10g protein per serving
    LowCarbohydrate, // < 15g carbohydrates per serving
    LowSugar, // < 5g sugar per serving
    LowSalt, // <= 120mg salt per serving
    LowTotalFat, // < 3g of total fat per serving
    NutriLevel, // Must be correct GGL sticker
    Healthy, // GGL must be A
    NoPreservative, // no preservative added
    Composition, // Must be correct composition
}
[System.Serializable]
public struct Claim
{
    public ClaimType claimType;
    public string claimDescription;
    public bool isValid;
    public Claim(ClaimType type, string description, bool isCorrect)
    {
        claimType = type;
        claimDescription = description;
        isValid = isCorrect;
    }

    public static bool CompareClaim(Claim thisClaim, Claim otherClaim)
    {
        if (thisClaim.claimType == otherClaim.claimType && thisClaim.isValid == otherClaim.isValid)
        {
            return true;
        }
        return false;
    }

    public static bool CompareClaimType(Claim thisClaim, Claim otherClaim)
    {
        return thisClaim.claimType == otherClaim.claimType;
    }

    public bool ContainsClaimInList(List<Claim> claimList)
    {
        foreach (Claim claim in claimList)
        {
            if (CompareClaim(claim, this))
            {
                return true;
            }
        }
        return false;
    }

    public bool ContainsClaimTypeInList(List<Claim> claimList)
    {
        foreach (Claim claim in claimList)
        {
            if (CompareClaimType(claim, this))
            {
                return true;
            }
        }
        return false;
    }
}
public class ClaimsOnFood : MonoBehaviour
{
    public List<Claim> claims;

    public bool CompareClaims(FoodItem foodInfo)
    {
        int claimsMatched = 0;
        List<Claim> foodClaims = foodInfo.GetFoodData().Claims;
        foreach (Claim claim in claims)
        {
            if (claim.ContainsClaimInList(foodClaims))
            {
                foodClaims.Remove(claim);
                claimsMatched++;
            }
        }
        return claimsMatched == foodClaims.Count;
    }

    public void AddClaim(ClaimType claimType, bool isValid)
    {
        Claim newClaim = new(claimType, "", isValid);
        claims.Add(newClaim);
    }
}
