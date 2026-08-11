using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [Header("Settings")]
    public int Level = 0;
    public List<LevelFoodData> FoodDataList;
    public List<FoodDataSO> FoodToSpawn;
    [HideInInspector] public List<FoodDataSO> FoodSpawned;
    public Transform FoodParent;
    public float delayBetweenClients = 0.5f;

    [Header("Debug")]
    public int index = 0;

    public event Action<Dialogues> OnDialogueTriggered;
    public event Action SendFoodEvent;
    public event Action LevelCompletedEvent;

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

    private void Start()
    {
        SendFoodEvent += SpawnNextFood;
    }

    private void OnDisable()
    {
        SendFoodEvent -= SpawnNextFood;
    }

    public void SpawnFood(LevelFoodData foodData, Transform parent)
    {
        FoodToSpawn.Clear();
        if (foodData.FoodData != null)
        {
            for (int i = 0; i < foodData.FoodData.Count; i++)
            {
                FoodDataSO foodDataSO = foodData.FoodData[i].GetRandomFood();
                FoodToSpawn.Add(foodDataSO);
            }

            for (int i = 0; i < FoodToSpawn.Count; i++)
            {
                GameObject foodInstance = Instantiate(FoodToSpawn[i].GetRandomPrefab(), parent);
                foodInstance.GetComponent<FoodItem>().SetFoodData(FoodToSpawn[i]);
                foodInstance.GetComponentInChildren<NutritionInfo>(true).SetNutrition(FoodToSpawn[i].Components);
                foodInstance.name = FoodToSpawn[i].FoodId;
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

    public void TriggerDialogue(Dialogues dialogue)
    {
        OnDialogueTriggered?.Invoke(dialogue);
    }

    public void TriggerDialogue(int index)
    {
        if (index < FoodDataList.Count)
        {
            TriggerDialogue(FoodDataList[index].Dialogue);
        }
        else
        {
            Debug.LogWarning($"Index {index} is out of bounds for FoodDataList.");
        }
    }

    public void TriggerSendFoodEvent()
    {
        SendFoodEvent?.Invoke();
    }

    public void TriggerLevelCompletedEvent()
    {
        LevelCompletedEvent?.Invoke();
    }
}

[Serializable]
public class LevelFoodData
{
    public Dialogues Dialogue;
    public List<FoodLevelList> FoodData;
}

[Serializable]
public class FoodLevelList
{
    [Serializable]
    public struct FoodChance
    {
        public FoodDataSO FoodData;
        public float Chance;
        public FoodChance(FoodDataSO foodData, float chance)
        {
            FoodData = foodData;
            Chance = chance;
        }
    }
    public List<FoodChance> foodChances;
    public FoodDataSO GetRandomFood()
    {
        if (foodChances == null || foodChances.Count == 0)
            return null;
        List<float> chances = new List<float>();
        foreach (var chance in foodChances)
        {
            chances.Add(chance.Chance);
        }
        int randomIndex = GetRandomUpgradeIndex(chances);
        return randomIndex >= 0 ? foodChances[randomIndex].FoodData : null;
    }

    public int GetRandomUpgradeIndex(List<float> chances)
    {
        if (chances == null || chances.Count == 0)
            return -1; // Or return 0 depending on your error fallback design

        // 1. Calculate total sum of all weights
        float totalWeight = 0f;
        for (int i = 0; i < chances.Count; i++)
        {
            totalWeight += chances[i];
        }

        if (totalWeight <= 0f)
            return 0;

        // 2. Roll a random number between 0 (inclusive) and totalWeight (exclusive)
        float roll = UnityEngine.Random.Range(0f, totalWeight);

        // 3. Step through items using index 'i' directly
        float cumulativeWeight = 0f;
        for (int i = 0; i < chances.Count; i++)
        {
            cumulativeWeight += chances[i];
            if (roll < cumulativeWeight)
            {
                return i; // Directly returns the correct index, even with equal values!
            }
        }

        // Fallback case due to floating point rounding precision
        return chances.Count - 1;
    }
}
