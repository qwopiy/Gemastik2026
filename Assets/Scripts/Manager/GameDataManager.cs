using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDataManager : MonoBehaviour
{
    public static GameData gameData = new();

    void Awake()
    {
        if (!System.IO.File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            SaveData();
        }
        ReadData();
    }

    public static void SaveData()
    {
        SaveDataToJSON();
    }

    public static void ReadData()
    {
        ReadDataFromJSON();
    }

    public static void SaveDataToJSON()
    {
        string data = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + "/SaveData.json";
        Debug.Log("Data saved to " + filePath);
        System.IO.File.WriteAllText(filePath, data);
    }

    public static void ReadDataFromJSON()
    {
        string filePath = Application.persistentDataPath + "/SaveData.json";
        string data = System.IO.File.ReadAllText(filePath);

        gameData = JsonUtility.FromJson<GameData>(data);
        Debug.Log("Data Loaded.");
    }

    public void DEBUG_SaveData(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SaveData();
            SaveDataToJSON();
        }
    }

    public void DEBUG_ReadData(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ReadDataFromJSON();
            ReadData();
        }
    }
}
public enum Endings
{
    Neutral,
    AllMistake,
    AllCorrect,
    Sugar,
    Salt,
    Fat,
    ExpiredOrDefect,
    WrongNutritionClaim,
    WrongCompositionClaim,
}

[Serializable]
public class GameData
{
    public int levelProgress = 0;

    // Unlocked Endings
    public bool neutralEndingUnlocked = false;
    public bool allMistakeEndingUnlocked = false;
    public bool allCorrectEndingUnlocked = false;
    public bool sugarEndingUnlocked = false;
    public bool saltEndingUnlocked = false;
    public bool fatEndingUnlocked = false;
    public bool defectOrExpiredEndingUnlocked = false;
    public bool wrongNutritionClaimEndingUnlocked = false;
    public bool wrongCompositionClaimEndingUnlocked = false;

}