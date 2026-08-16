using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientData", menuName = "ScriptableObjects/ClientData", order = 1)]
public class ClientDataSO : ScriptableObject
{
    public Dialogues Dialogue;
    public int AmountToSpawn = 3;
    [Tooltip("Folder path relative to the Resources folder, e.g., Assets/Resources + 'FoodData/Level1'")]
    public string foodFolder = "FoodData/Level1";

    //[HideInInspector]
    public List<FoodDataSO> FoodData;

    public void LoadFoodDataFromFolder()
    {
        if (foodFolder != null)
        {
            FoodData = new List<FoodDataSO>(Resources.LoadAll<FoodDataSO>(foodFolder));
        }
        else
        {
            Debug.LogWarning("Food folder is not assigned.");
        }
    }

    public FoodDataSO GetRandomFood()
    {
        if (FoodData == null || FoodData.Count == 0)
        {
            Debug.LogWarning("FoodData list is empty or not assigned.");
            return null;
        }
        int randomIndex = UnityEngine.Random.Range(0, FoodData.Count);
        return FoodData[randomIndex];
    }
}