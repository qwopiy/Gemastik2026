using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AddGGLReasonToSO : EditorWindow
{
    private DefaultAsset folderAsset;
    private string folderPath = "Assets/GeneratedItems";

    private int[] GGLCounter = new int[4]{0, 0, 0, 0};

    [MenuItem("Tools/Add GGL Reason to SO")]
    public static void ShowWindow()
    {
        GetWindow<AddGGLReasonToSO>("GGL Reason Adder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Add GGL Reason to ScriptableObjects", EditorStyles.boldLabel);
        folderAsset = (DefaultAsset)EditorGUILayout.ObjectField("Folder to Process", folderAsset, typeof(DefaultAsset), false);
        folderPath = EditorGUILayout.TextField("Save Folder Path", folderPath);

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

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

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

                // Create a new instance of the ScriptableObject
                FoodDataSO newAsset = CreateInstance<FoodDataSO>();

                newAsset.CopyValues(childAsset); // Copy properties from the existing asset

                SetGGLToSO(newAsset);

                // Save the asset file to the project
                string newAssetPath = $"{folderPath}/Item_{newAsset.FoodId}.asset";
                //Debug.Log(newAssetPath);
                AssetDatabase.CreateAsset(newAsset, newAssetPath);

                //Debug.Log($"Iterating file: {childAsset.name} | Type: {childAsset.GetType()} | Path: {assetPath}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //Debug.Log("ScriptableObjects created successfully in " + folderPath);
        Debug.Log($"GGL Sticker Count: A: {GGLCounter[0]}, B: {GGLCounter[1]}, C: {GGLCounter[2]}, D: {GGLCounter[3]}");
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

        GGLSticker maxGGL = (GGLSticker) Mathf.Max((int)sugarGGL, Mathf.Max((int)saltGGL, (int)fatGGL));
        AddToGGLCounter(maxGGL);

        List<GGLReason> reasons = new List<GGLReason>();
        if (maxGGL == sugarGGL) reasons.Add(GGLReason.Sugar);
        if (maxGGL == saltGGL) reasons.Add(GGLReason.Salt);
        if (maxGGL == fatGGL) reasons.Add(GGLReason.Fat);

        asset.GGLRating = maxGGL;
        asset.GGLReasons = reasons;
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
