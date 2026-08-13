using System.IO;
using UnityEditor;
using UnityEngine;

public class SetLevelSO : EditorWindow
{
    private DefaultAsset folderAsset;
    private string folderPath = "Assets/GeneratedItems";
    private int randomObjCount = 10;
    private int levelToSet = 1; // Default level to set for the ScriptableObjects
    private bool isExpired = false;
    private bool isDefect = false;


    private int[] GGLCounter = new int[4] { 0, 0, 0, 0 };

    [MenuItem("Tools/Set Level SO")]
    public static void ShowWindow()
    {
        GetWindow<SetLevelSO>("Set Level SO");
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Level for ScriptableObjects", EditorStyles.boldLabel);
        folderAsset = (DefaultAsset)EditorGUILayout.ObjectField("Folder to Process", folderAsset, typeof(DefaultAsset), false);
        randomObjCount = EditorGUILayout.IntField("Random Object Count", randomObjCount);
        folderPath = EditorGUILayout.TextField("Save Folder Path", folderPath);
        levelToSet = EditorGUILayout.IntField("Level to Set", levelToSet);
        isExpired = EditorGUILayout.Toggle("Set Expired", isExpired);
        isDefect = EditorGUILayout.Toggle("Set Defective", isDefect);

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
    }

    private void Process()
    {
        // 1. Get the project-relative path of the folder (e.g., "Assets/MyFolder")
        string ItemToProcessPath = AssetDatabase.GetAssetPath(folderAsset);

        // 2. Find all assets inside that specific folder path
        // "t:FoodDataSO" grabs only FoodDataSO assets
        string[] assetGuids = AssetDatabase.FindAssets("t:FoodDataSO", new[] { ItemToProcessPath });
        string[] randomGuids = GetRandomElements(assetGuids, randomObjCount);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        int index = 0;
        // 3. Iterate over every file found
        foreach (string guid in randomGuids)
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

                // Create a new instance of the ScriptableObject
                FoodDataSO newAsset = CreateInstance<FoodDataSO>();

                newAsset.CopyValues(childAsset); // Copy properties from the existing asset

                AddSOToLevel(newAsset, levelToSet); // Set the level for the new asset)

                // Save the asset file to the project
                string newAssetPath = $"{folderPath}/Item_{index}_L{levelToSet}_ID{newAsset.FoodId}.asset";
                //Debug.Log(newAssetPath);
                AssetDatabase.CreateAsset(newAsset, newAssetPath);

                index++;
                //Debug.Log($"Iterating file: {childAsset.name} | Type: {childAsset.GetType()} | Path: {assetPath}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //Debug.Log("ScriptableObjects created successfully in " + folderPath);

        Debug.Log($"GGL Sticker Count: A: {GGLCounter[0]}, B: {GGLCounter[1]}, C: {GGLCounter[2]}, D: {GGLCounter[3]}");
    }

    private void AddSOToLevel(FoodDataSO so, int level)
    {
        AddToGGLCounter(so.GGLRating);
        switch (level) 
        {
            case 1:
                // Add logic for level 1
                NormalizeGGL(so);
                break;
            case 2:
                SetExpiryAndDefect(so);
                break;
            case 3:
            case 4:
                SetExpiryAndDefect(so);
                SetRandomClaimToSO(so);
                break;
            default:
                throw new System.ArgumentException("Invalid level specified. Level must be 1, 2, 3, or 4.");
        }
    }

    private void NormalizeGGL(FoodDataSO so)
    {
        if (so == null) return;

        bool isGram = so.Components[0].AttributeFields[0].Value.Contains(" g");
        float servingSize = float.Parse(so.Components[0].AttributeFields[0].Value.Replace(" g", "").Replace(" mL", ""));

        float newServingSizeRatio = servingSize / 100f;

        string unit = isGram ? "g" : "mL";
        so.Components[0].AttributeFields[0].Value = $"100 {unit}"; // Set the serving size to 100g

        // Normalize GGL values based on the new serving size ratio
        float sugar = float.Parse(so.Components[1].AttributeFields[4].Value);
        float salt = float.Parse(so.Components[1].AttributeFields[5].Value);
        float fat = float.Parse(so.Components[1].AttributeFields[1].Value);

        so.Components[1].AttributeFields[4].Value = (sugar * newServingSizeRatio).ToString("F0");
        so.Components[1].AttributeFields[5].Value = (salt * newServingSizeRatio).ToString("F0");
        so.Components[1].AttributeFields[1].Value = (fat * newServingSizeRatio).ToString("F1");
    }

    private void SetExpiryAndDefect(FoodDataSO so)
    {
        so.ExpiryDate = Calendar.GetRandomDate(isExpired);

        so.IsExpired = isExpired;
        so.IsDefect = isDefect;
    }

    private void SetRandomClaimToSO(FoodDataSO so)
    {
        int randomClaimCount = Random.Range(1, 4); // 1 to 3 claims

        for (int i = 0; i < randomClaimCount; i++) 
        {
            int randomClaimType = Random.Range(1, 13); // 1 to 12 
            string claimDescription = ClaimStringsDatabase.GetRandomDescription((ClaimType)randomClaimType);

            Claim newClaim = new ((ClaimType)randomClaimType, claimDescription, CheckValidity(so, (ClaimType)randomClaimType, claimDescription));
            so.Claims.Add(newClaim);
        }
    }

    private bool CheckValidity(FoodDataSO so, ClaimType claim, string description = "A")
    {
        float servingSize = float.Parse(so.Components[0].AttributeFields[0].Value.Replace(" g", "").Replace(" mL", ""));
        switch (claim)
        {
            case ClaimType.CalorieFree:
                return float.Parse(so.Components[0].AttributeFields[1].Value) < 5f * servingSize / 100f;
            case ClaimType.HighProtein:
                return float.Parse(so.Components[1].AttributeFields[2].Value) > 10f * servingSize / 100f;
            case ClaimType.LowCarbohydrate:
                return float.Parse(so.Components[1].AttributeFields[3].Value) < 15f * servingSize / 100f;
            case ClaimType.SugarFree:
                return float.Parse(so.Components[1].AttributeFields[4].Value) < 0.5f * servingSize / 100f;
            case ClaimType.LowSugar:
                return float.Parse(so.Components[1].AttributeFields[4].Value) < 5f * servingSize / 100f;
            case ClaimType.LowSalt:
                return float.Parse(so.Components[1].AttributeFields[5].Value) <= 120f * servingSize / 100f;
            case ClaimType.LowTotalFat:
                return float.Parse(so.Components[1].AttributeFields[1].Value) < 3f * servingSize / 100f;
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

    private GGLSticker GetRandomGGL()
    {
        int randomValue = Random.Range(1, 5); // 1 to 4
        return (GGLSticker)randomValue;
    }

    private string[] GetRandomElements(string[] source, int count)
    {
        // 1. Initialize the new array with the set amount
        string[] newArray = new string[count];

        for (int i = 0; i < count; i++)
        {
            // 2. Pick a random index from the source array
            int randomIndex = Random.Range(0, source.Length);

            // 3. Assign it to the new array
            newArray[i] = source[randomIndex];
        }

        return newArray;
    }

    private void AddToGGLCounter(GGLSticker ggl)
    {
        switch (ggl)
        {
            case GGLSticker.A:
                GGLCounter[0]++;
                break;
            case GGLSticker.B:
                GGLCounter[1]++;
                break;
            case GGLSticker.C:
                GGLCounter[2]++;
                break;
            case GGLSticker.D:
                GGLCounter[3]++;
                break;
        }
    }
}
