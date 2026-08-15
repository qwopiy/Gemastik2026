using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class UpdateFoodSO : EditorWindow
{
    private DefaultAsset folderAsset;


    [MenuItem("Tools/Update Food SO")]
    public static void ShowWindow()
    {
        GetWindow<UpdateFoodSO>("Update Food SO");
    }

    private void OnGUI()
    {
        GUILayout.Label("Update Food ScriptableObjects", EditorStyles.boldLabel);
        folderAsset = (DefaultAsset)EditorGUILayout.ObjectField("Folder to Process", folderAsset, typeof(DefaultAsset), false);

        if (GUILayout.Button("Process and Create SOs"))
        {
            if (folderAsset != null)
            {
                Process();
            }
            else
            {
                Debug.LogError("Please assign a folder first!");
            }
        }

        if (GUILayout.Button("Normalize Nutrition To 100g/ml"))
        {
            if (folderAsset != null)
            {
                NormalizeNutrition();
            }
            else
            {
                Debug.LogError("Please assign a folder first!");
            }
        }

        //if (GUILayout.Button("Remove Duplicate Claims"))
        //{
        //    if (folderAsset != null)
        //    {
        //        RemoveDuplicateClaims();
        //    }
        //    else
        //    {
        //        Debug.LogError("Please assign a folder first!");
        //    }
        //}
    }

    private void Process()
    {
        // 1. Get the project-relative path of the folder (e.g., "Assets/MyFolder")
        string ItemToProcessPath = AssetDatabase.GetAssetPath(folderAsset);

        // 2. Find all assets inside that specific folder path
        // "t:FoodDataSO" grabs only FoodDataSO assets
        string[] assetGuids = AssetDatabase.FindAssets("t:FoodDataSO", new[] { ItemToProcessPath });

        // 3. Iterate over every file found
        foreach (string guid in assetGuids)
        {
            // 4. Convert the unique ID back into a string path
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // 5. Skip the root folder asset itself (AssetDatabase includes it in the search)
            if (assetPath == ItemToProcessPath) continue;

            // 6. Optional: Load the asset if you need to inspect or modify its properties
            FoodDataSO childAsset = AssetDatabase.LoadAssetAtPath<FoodDataSO>(assetPath);


            if (childAsset != null)
            {
                // Do your custom work here
                UpdateServingSize(childAsset);
                SetGGLToSO(childAsset);
                SetRandomClaimToSO(childAsset);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //Debug.Log("ScriptableObjects created successfully in " + folderPath);
    }

    private void NormalizeNutrition()
    {
        // 1. Get the project-relative path of the folder (e.g., "Assets/MyFolder")
        string ItemToProcessPath = AssetDatabase.GetAssetPath(folderAsset);

        // 2. Find all assets inside that specific folder path
        // "t:FoodDataSO" grabs only FoodDataSO assets
        string[] assetGuids = AssetDatabase.FindAssets("t:FoodDataSO", new[] { ItemToProcessPath });

        // 3. Iterate over every file found
        foreach (string guid in assetGuids)
        {
            // 4. Convert the unique ID back into a string path
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // 5. Skip the root folder asset itself (AssetDatabase includes it in the search)
            if (assetPath == ItemToProcessPath) continue;

            // 6. Optional: Load the asset if you need to inspect or modify its properties
            FoodDataSO childAsset = AssetDatabase.LoadAssetAtPath<FoodDataSO>(assetPath);


            if (childAsset != null)
            {
                NormalizeNutritionTo100gml(childAsset);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //Debug.Log("ScriptableObjects created successfully in " + folderPath);
    }

    //private void RemoveDuplicateClaims()
    //{
    //    // 1. Get the project-relative path of the folder (e.g., "Assets/MyFolder")
    //    string ItemToProcessPath = AssetDatabase.GetAssetPath(folderAsset);

    //    // 2. Find all assets inside that specific folder path
    //    // "t:FoodDataSO" grabs only FoodDataSO assets
    //    string[] assetGuids = AssetDatabase.FindAssets("t:FoodDataSO", new[] { ItemToProcessPath });

    //    // 3. Iterate over every file found
    //    foreach (string guid in assetGuids)
    //    {
    //        // 4. Convert the unique ID back into a string path
    //        string assetPath = AssetDatabase.GUIDToAssetPath(guid);

    //        // 5. Skip the root folder asset itself (AssetDatabase includes it in the search)
    //        if (assetPath == ItemToProcessPath) continue;

    //        // 6. Optional: Load the asset if you need to inspect or modify its properties
    //        FoodDataSO childAsset = AssetDatabase.LoadAssetAtPath<FoodDataSO>(assetPath);


    //        if (childAsset != null)
    //        {
    //            RemoveDuplicateClaimsFromSO(childAsset);
    //        }
    //    }

    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //    //Debug.Log("ScriptableObjects created successfully in " + folderPath);
    //}

    //private void RemoveDuplicateClaimsFromSO(FoodDataSO so)
    //{
    //    HashSet<ClaimType> uniqueClaimTypes = new HashSet<ClaimType>();
    //    List<Claim> uniqueClaims = new List<Claim>();
    //    foreach (var claim in so.Claims)
    //    {
    //        if (!uniqueClaimTypes.Contains(claim.claimType))
    //        {
    //            uniqueClaimTypes.Add(claim.claimType);
    //            uniqueClaims.Add(claim);
    //        }
    //    }
    //    so.Claims = uniqueClaims;
    //}
    private void NormalizeNutritionTo100gml(FoodDataSO so)
    {
        int validServingSize = 100; // Normalize to 100 ml or g

        float ratio = validServingSize / float.Parse(so.Components[0].AttributeFields[0].Value.Split(' ')[0]);

        bool isGram = so.Components[0].AttributeFields[0].Value.Contains(" g");
        so.Components[0].AttributeFields[0].Value = validServingSize.ToString() + (isGram ? " g" : " mL");

        UpdateSOComponent(so, 0, 2, ratio, "F0");
        UpdateSOComponent(so, 1, 0, ratio, "F1");
        UpdateSOComponent(so, 1, 1, ratio, "F1");
        UpdateSOComponent(so, 1, 2, ratio, "F0");
        UpdateSOComponent(so, 1, 3, ratio, "F0");
        UpdateSOComponent(so, 1, 4, ratio, "F0");
        UpdateSOComponent(so, 1, 5, ratio, "F0");

        // Set Composition to null
        so.Components[2].AttributeFields[0].Value = "";

        GGLSticker sugar = GetGGL(float.Parse(so.Components[1].AttributeFields[4].Value), GGLReason.Sugar, validServingSize);
        GGLSticker salt = GetGGL(float.Parse(so.Components[1].AttributeFields[5].Value), GGLReason.Salt, validServingSize);
        GGLSticker fat = GetGGL(float.Parse(so.Components[1].AttributeFields[1].Value), GGLReason.Fat, validServingSize);

        GGLSticker maxGGL = (GGLSticker)Mathf.Max((int)sugar, Mathf.Max((int)salt, (int)fat));

        so.GGLRating = maxGGL;
    }
    private void UpdateServingSize(FoodDataSO so)
    {
        List<int> listOfValidSizes = new()
        {
            10,
            20,
            25,
            50,
            100,
            200
        };
        int randomIndex = Random.Range(0, listOfValidSizes.Count);
        int randomServingSize = listOfValidSizes[randomIndex];

        float ratio = randomServingSize / float.Parse(so.Components[0].AttributeFields[0].Value.Split(' ')[0]);

        bool isGram = so.Components[0].AttributeFields[0].Value.Contains(" g");
        so.Components[0].AttributeFields[0].Value = randomServingSize.ToString() + (isGram ? " g" : " mL");

        UpdateSOComponent(so, 0, 2, ratio, "F0");
        UpdateSOComponent(so, 1, 0, ratio, "F1");
        UpdateSOComponent(so, 1, 1, ratio, "F1");
        UpdateSOComponent(so, 1, 2, ratio, "F0");
        UpdateSOComponent(so, 1, 3, ratio, "F0");
        UpdateSOComponent(so, 1, 4, ratio, "F0");
        UpdateSOComponent(so, 1, 5, ratio, "F0");

        // Set Composition to null
        so.Components[2].AttributeFields[0].Value = "";

        GGLSticker sugar = GetGGL(float.Parse(so.Components[1].AttributeFields[4].Value), GGLReason.Sugar, randomServingSize);
        GGLSticker salt = GetGGL(float.Parse(so.Components[1].AttributeFields[5].Value), GGLReason.Salt, randomServingSize);
        GGLSticker fat = GetGGL(float.Parse(so.Components[1].AttributeFields[1].Value), GGLReason.Fat, randomServingSize);

        GGLSticker maxGGL = (GGLSticker)Mathf.Max((int)sugar, Mathf.Max((int)salt, (int)fat));

        so.GGLRating = maxGGL;
    }

    private void UpdateSOComponent(FoodDataSO so, int componentIndex, int attributeIndex, float ratio, string stringParam)
    {
        float originalValue = float.Parse(so.Components[componentIndex].AttributeFields[attributeIndex].Value);

        float newValue = originalValue * ratio;
        so.Components[componentIndex].AttributeFields[attributeIndex].Value = newValue.ToString(stringParam);
    }

    private GGLSticker GetGGL(float value, GGLReason type, float servingSize)
    {
        float valuePerServing = value / servingSize * 100; // Normalize to per 100 ml or g)

        switch (type)
        {
            case GGLReason.Sugar:
                if (valuePerServing <= 1) return GGLSticker.A;
                if (valuePerServing > 1 && valuePerServing <= 5) return GGLSticker.B;
                if (valuePerServing > 5 && valuePerServing <= 10) return GGLSticker.C;
                if (valuePerServing > 10) return GGLSticker.D;
                break;
            case GGLReason.Salt:
                if (valuePerServing <= 5) return GGLSticker.A;
                if (valuePerServing > 5 && valuePerServing <= 120) return GGLSticker.B;
                if (valuePerServing > 120 && valuePerServing <= 500) return GGLSticker.C;
                if (valuePerServing > 500) return GGLSticker.D;
                break;
            case GGLReason.Fat:
                if (valuePerServing <= 0.7f) return GGLSticker.A;
                if (valuePerServing > 0.7f && valuePerServing <= 1.2f) return GGLSticker.B;
                if (valuePerServing > 1.2f && valuePerServing <= 2.8f) return GGLSticker.C;
                if (valuePerServing > 2.8f) return GGLSticker.D;
                break;
        }
        return GGLSticker.A; // Default case
    }

    private void SetRandomClaimToSO(FoodDataSO so)
    {
        so.Claims.Clear(); // Clear existing claims before adding new ones
        //int randomClaimCount = Random.Range(0, 4); // 0 to 3 claims
        List<int> weightedRandom = new()
        {
            30,
            30,
            25,
            15,
        };

        int randomNumber = Random.Range(0, 100);
        int claimCount = 0;
        for (int i = 0; i < weightedRandom.Count; i++)
        {
            int item = weightedRandom[i];
            if (randomNumber < item)
            {
                claimCount = i;
                break;
            }
            randomNumber -= item;
        }

        List<Claim> randomClaims = new();

        // prevents duplicate claims by checking if the claim already exists in the list
        while (randomClaims.Count < claimCount)
        {
            int randomClaimType = Random.Range(1, 12); // 1 to 11 
            string claimDescription = ClaimStringsDatabase.GetRandomDescription((ClaimType)randomClaimType);

            Claim newClaim = new((ClaimType)randomClaimType, claimDescription, CheckValidity(so, (ClaimType)randomClaimType, claimDescription));
            if (!newClaim.ContainsClaimTypeInList(randomClaims))
            {
                randomClaims.Add(newClaim);
            }
        } 

        so.Claims = randomClaims;
    }
    private bool CheckValidity(FoodDataSO so, ClaimType claim, string description = "A")
    {
        switch (claim)
        {
            case ClaimType.CalorieFree:
                return float.Parse(so.Components[0].AttributeFields[2].Value) < 5f;
            case ClaimType.HighProtein:
                return float.Parse(so.Components[1].AttributeFields[2].Value) > 10f;
            case ClaimType.LowCarbohydrate:
                return float.Parse(so.Components[1].AttributeFields[3].Value) < 15f;
            case ClaimType.SugarFree:
                return float.Parse(so.Components[1].AttributeFields[4].Value) < 0.5f;
            case ClaimType.LowSugar:
                return float.Parse(so.Components[1].AttributeFields[4].Value) < 5f;
            case ClaimType.LowSalt:
                return float.Parse(so.Components[1].AttributeFields[5].Value) <= 120f;
            case ClaimType.LowTotalFat:
                return float.Parse(so.Components[1].AttributeFields[0].Value) < 3f;
            case ClaimType.NutriLevel:
                return so.GGLRating.ToString() == description[^1].ToString();
            case ClaimType.Healthy:
                return so.GGLRating == GGLSticker.A;
            case ClaimType.NoPreservative:
            case ClaimType.Composition:
                return true; // masukin manual
            case ClaimType.None:
                return false;
        }
        return false;
    }

    private void SetGGLToSO(FoodDataSO asset)
    {
        // rules: per 100 ml atau 100 gram

        // gula <= 1 = a,
        // gula > 1 && gula <= 5 = b,
        // gula > 5 && gula <= 10 = c,
        // gula > 10 = d

        // garam <= 5 = a,
        // garam > 5 && garam <= 120 = b,
        // garam > 120 && garam <= 500 = c,
        // garam > 500 = d

        // lemak <= 0.7 = a,
        // lemak > 0.7 && lemak <= 1.2 = b,
        // lemak > 1.2 && lemak <= 2.8 = c,
        // lemak > 2.8 = d

        string servingSizeString = asset.Components[0].AttributeFields[0].Value; // "ml" or "g"

        float servingSize = float.Parse(servingSizeString.Replace(" mL", "").Replace(" g", ""));
        float sugar = float.Parse(asset.Components[1].AttributeFields[4].Value);
        float salt = float.Parse(asset.Components[1].AttributeFields[5].Value);
        float fat = float.Parse(asset.Components[1].AttributeFields[1].Value);

        GGLSticker sugarGGL = GetGGL(sugar, GGLReason.Sugar, servingSize);
        GGLSticker saltGGL = GetGGL(salt, GGLReason.Salt, servingSize);
        GGLSticker fatGGL = GetGGL(fat, GGLReason.Fat, servingSize);

        GGLSticker minGGL = (GGLSticker)Mathf.Min((int)sugarGGL, Mathf.Min((int)saltGGL, (int)fatGGL));

        List<GGLReason> reasons = new List<GGLReason>();
        if (minGGL == sugarGGL) reasons.Add(GGLReason.Sugar);
        if (minGGL == saltGGL) reasons.Add(GGLReason.Salt);
        if (minGGL == fatGGL) reasons.Add(GGLReason.Fat);

        asset.GGLRating = minGGL;
        asset.GGLReasons = reasons;
    }
}
