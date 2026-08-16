using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [Header("Settings")]
    public int Level = 0;
    public int TotalClients = 0;
    public List<ClientDataSO> ClientDataList;
    [HideInInspector] public List<FoodDataSO> FoodSpawned;
    public GameObjectAnchorSO FoodParent;
    public float delayBetweenClients = 0.5f;

    [Header("Debug")]
    public int index = 0;
    public List<FoodDataSO> FoodToSpawn;
    public List<int> ClientServedIndex;

    public event Action<Dialogues> OnDialogueTriggered;
    public event Action SendFoodEvent;
    public event Action LevelStartedEvent;
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
        foreach (var foodList in ClientDataList)
        {
            foodList.LoadFoodDataFromFolder();
        }
        SendFoodEvent += SpawnNextFood;
    }

    private void OnDisable()
    {
        SendFoodEvent -= SpawnNextFood;
    }

    public void SpawnFood(ClientDataSO foodData, Transform parent)
    {
        FoodToSpawn.Clear();
        if (foodData.FoodData != null)
        {
            for (int i = 0; i < foodData.AmountToSpawn; i++)
            {
                FoodDataSO foodDataSO = foodData.GetRandomFood();
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
        if (index < TotalClients)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, ClientDataList.Count);

            } while (ClientServedIndex.Contains(randomIndex));

            SpawnFood(ClientDataList[randomIndex], FoodParent.value.transform);

            ClientServedIndex.Add(randomIndex);
            index++;
        }
        else
        {
            EndingManager.Instance.CheckProgress();
            TriggerLevelCompletedEvent();
            Debug.Log("All food items have been spawned.");
        }
    }

    public void TriggerDialogue(Dialogues dialogue)
    {
        OnDialogueTriggered?.Invoke(dialogue);
    }

    public void TriggerDialogue(int index)
    {
        if (index < ClientDataList.Count)
        {
            TriggerDialogue(ClientDataList[index].Dialogue);
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

    public void TriggerLevelStartedEvent()
    {
        LevelStartedEvent?.Invoke();
    }
    public void TriggerLevelCompletedEvent()
    {
        LevelCompletedEvent?.Invoke();
    }
}