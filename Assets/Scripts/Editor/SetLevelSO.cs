using System.IO;
using UnityEditor;
using UnityEngine;

public class SetLevelSO : EditorWindow
{
    private DefaultAsset folderAsset;
    private string folderPath = "Assets/Resources/FoodData/";
    private int isNormalAmount = 0;
    private int isExpiredAmount = 0;
    private int isDefectAmount = 0;
    private int isBothAmount = 0;


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
        folderPath = EditorGUILayout.TextField("Save Folder Path", folderPath);
        isNormalAmount = EditorGUILayout.IntField("Normal Items", isNormalAmount);
        isExpiredAmount = EditorGUILayout.IntField("Expired Items", isExpiredAmount);
        isDefectAmount = EditorGUILayout.IntField("Defective Items", isDefectAmount);
        isBothAmount = EditorGUILayout.IntField("Both Expired and Defective", isBothAmount);

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

        int randomObjCount = isNormalAmount + isExpiredAmount + isDefectAmount + isBothAmount;
        string[] randomGuids = GetRandomElements(assetGuids, randomObjCount);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        int index = 0;
        // 3. Iterate over every file found
        for (int i = 0; i < isNormalAmount; i++)
        {
            AddSO(index, ItemToProcessPath, randomGuids, isExpired: false, isDefect: false);
            index++;
        }
        for (int i = 0; i < isExpiredAmount; i++)
        {
            AddSO(index, ItemToProcessPath, randomGuids, isExpired: true, isDefect: false);
            index++;
        }
        for (int i = 0; i < isDefectAmount; i++)
        {
            AddSO(index, ItemToProcessPath, randomGuids, isExpired: false, isDefect: true);
            index++;
        }
        for (int i = 0; i < isBothAmount; i++)
        {
            AddSO(index, ItemToProcessPath, randomGuids, isExpired: true, isDefect: true);
            index++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //Debug.Log("ScriptableObjects created successfully in " + folderPath);

        Debug.Log($"GGL Sticker Count: A: {GGLCounter[0]}, B: {GGLCounter[1]}, C: {GGLCounter[2]}, D: {GGLCounter[3]}");
    }

    private void AddSO(int index, string ItemToProcessPath, string[] randomGuids, bool isExpired, bool isDefect)
    {
        string guid = randomGuids[index];
        // 4. Convert the unique ID back into a string path
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);

        // 5. Skip the root folder asset itself (AssetDatabase includes it in the search)
        if (assetPath == ItemToProcessPath) return;

        // 6. Optional: Load the asset if you need to inspect or modify its properties
        FoodDataSO childAsset = AssetDatabase.LoadAssetAtPath<FoodDataSO>(assetPath);


        if (childAsset != null)
        {
            // Do your custom work here

            // Create a new instance of the ScriptableObject
            FoodDataSO newAsset = CreateInstance<FoodDataSO>();

            newAsset.CopyValues(childAsset); // Copy properties from the existing asset

            newAsset.IsExpired = isExpired;
            newAsset.IsDefect = isDefect;
            AddSOToLevel(newAsset); // Set the level for the new asset)

            // Save the asset file to the project
            string newAssetPath = $"{folderPath}/Item_{index}_E{(newAsset.IsExpired? 1 : 0)}_D{(newAsset.IsDefect? 1 : 0)}.asset";
            //Debug.Log(newAssetPath);
            AssetDatabase.CreateAsset(newAsset, newAssetPath);

            //Debug.Log($"Iterating file: {childAsset.name} | Type: {childAsset.GetType()} | Path: {assetPath}");
        }
    }
    private void AddSOToLevel(FoodDataSO so)
    {
        AddToGGLCounter(so.GGLRating);
        SetExpiryAndDefect(so);
    }


    private void SetExpiryAndDefect(FoodDataSO so)
    {
        so.ExpiryDate = Calendar.GetRandomDate(so.IsExpired);
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
