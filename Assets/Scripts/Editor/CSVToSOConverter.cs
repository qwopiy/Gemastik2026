using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVToSOConverter : EditorWindow
{
    private TextAsset csvFile;

    [MenuItem("Tools/Convert CSV to ScriptableObjects")]
    public static void ShowWindow()
    {
        GetWindow<CSVToSOConverter>("CSV Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert CSV to Item ScriptableObjects", EditorStyles.boldLabel);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);

        if (GUILayout.Button("Process and Create SOs"))
        {
            if (csvFile != null)
            {
                ConvertCSV();
            }
            else
            {
                Debug.LogError("Please assign a CSV file first!");
            }
        }
    }

    private void ConvertCSV()
    {
        // Split file into lines
        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Define save directory
        string folderPath = "Assets/GeneratedItems";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Skip index 0 because it contains the headers
        for (int i = 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(';');

            if (row.Length < 11) continue; // Ensure row has enough data

            // Create a new instance of the ScriptableObject
            FoodDataSO asset = CreateInstance<FoodDataSO>();

            asset.FoodId = row[0];

            // Parse data matching your Excel headers
            asset.SetComponents(
                new List<string> 
                { 
                    row[1], 
                    row[2], 
                    row[3], 
                    row[4], 
                    row[5], 
                    row[6], 
                    row[7], 
                    row[8], 
                    row[9], 
                    row[10],
                }
            );
            

            // Save the asset file to the project
            string assetPath = $"{folderPath}/Item_{asset.FoodId}.asset";
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObjects created successfully in " + folderPath);
    }
}
