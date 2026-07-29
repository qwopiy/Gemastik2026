using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [Header("Settings")]
    public List<LevelFoodData> FoodDataList;
    public Transform FoodParent;

    [Header("Debug")]
    public int index = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            index = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SpawnFood(LevelFoodData foodData, Transform parent)
    {
        if (foodData.FoodData != null)
        {
            for (int i = 0; i < foodData.FoodData.Count; i++)
            {
                GameObject foodInstance = Instantiate(foodData.FoodData[i].FoodPrefab, parent);
                foodInstance.GetComponent<FoodItem>().SetFoodData(foodData.FoodData[i]);
                foodInstance.GetComponentInChildren<NutritionInfo>().SetNutrition(foodData.FoodData[i].Components);
                foodInstance.name = foodData.FoodData[i].FoodId;
            }
        }
        else
        {
            Debug.LogWarning($"FoodData is not assigned");
        }
    }

    public void SpawnNextFood()
    {
        if (index < FoodDataList.Count)
        {
            SpawnFood(FoodDataList[index], FoodParent);
            index++;
        }
        else
        {
            Debug.Log("All food items have been spawned.");
        }
    }
}
[System.Serializable]
public class LevelFoodData
{
    public List<FoodDataSO> FoodData;
    public LevelFoodData(List<FoodDataSO> foodData)
    {
        FoodData = foodData;
    }
}
