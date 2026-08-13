using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NormalizeServingSize : EditorWindow
{
    private DefaultAsset folderAsset;


    [MenuItem("Tools/Normalize Serving Size")]
    public static void ShowWindow()
    {
        GetWindow<NormalizeServingSize>("Normalize Serving Size");
    }

    private void OnGUI()
    {
        GUILayout.Label("Normalize Serving Size for ScriptableObjects", EditorStyles.boldLabel);
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
                UpdateFoodDataSO(childAsset);

            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //Debug.Log("ScriptableObjects created successfully in " + folderPath);
    }

    private void UpdateFoodDataSO(FoodDataSO so)
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
}
