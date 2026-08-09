using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [Header("Settings")]
    public int Level = 0;
    public List<LevelFoodData> FoodDataList;
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
        if (foodData.FoodData != null)
        {
            for (int i = 0; i < foodData.FoodData.Count; i++)
            {
                GameObject foodInstance = Instantiate(foodData.FoodData[i].FoodPrefab, parent);
                foodInstance.GetComponent<FoodItem>().SetFoodData(foodData.FoodData[i]);
                foodInstance.GetComponentInChildren<NutritionInfo>(true).SetNutrition(foodData.FoodData[i].Components);
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

[System.Serializable]
public class LevelFoodData
{
    public Dialogues Dialogue;
    public List<FoodDataSO> FoodData;
    public LevelFoodData(List<FoodDataSO> foodData)
    {
        FoodData = foodData;
    }
}
