using System.Collections.Generic;
using UnityEngine;

public enum MistakeType
{
    None,
    Sugar,
    Salt,
    Fat,
    Expired,
    Defect,
    WrongNutritionClaim,
    WrongCompositionClaim,

}
public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;

    [Header("Timer Settings")]
    public float timeLimit = 60f; // Example time limit in seconds
    public float elapsedTime = 0f;
    public bool isTimerRunning = false;

    [Header("Ending Variables")]
    public int corrects = 0;
    public int mistakes = 0;
    public List<MistakeType> mistakeTypes = new();
    public Endings currentEnding = Endings.Neutral;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LevelManager.Instance.LevelStartedEvent += StartTimer;
        LevelManager.Instance.LevelCompletedEvent += StopTimer;
    }

    private void OnDisable()
    {
        LevelManager.Instance.LevelStartedEvent -= StartTimer;
        LevelManager.Instance.LevelCompletedEvent -= StopTimer;
    }
    private void Update()
    {
        if (isTimerRunning) 
            elapsedTime += Time.deltaTime;  
    }

    private void StartTimer()
    {
        isTimerRunning = true;
    }

    private void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetEndingData()
    {
        corrects = 0;
        mistakes = 0;
        elapsedTime = 0f;
        mistakeTypes.Clear();
    }
    public void CalculateEnding()
    {
        if (mistakes == 0) 
        {
            if (elapsedTime < timeLimit)
            {
                currentEnding = Endings.PerfectSpeedrunner;
                return;
            }

            currentEnding = Endings.AllCorrect;
            return;
        }
        else if (corrects == 0)
        {
            currentEnding = Endings.AllMistake;
            return;
        }

        List<int> mistakesCount = new()
        {
            mistakeTypes.FindAll(x => x == MistakeType.Sugar).Count,
            mistakeTypes.FindAll(x => x == MistakeType.Salt).Count,
            mistakeTypes.FindAll(x => x == MistakeType.Fat).Count,
            mistakeTypes.FindAll(x => x == MistakeType.Expired).Count,
            mistakeTypes.FindAll(x => x == MistakeType.Defect).Count,
            mistakeTypes.FindAll(x => x == MistakeType.WrongNutritionClaim).Count,
            mistakeTypes.FindAll(x => x == MistakeType.WrongCompositionClaim).Count
        };

        MistakeType mistakeType = MistakeType.None;
        int maxMistakeCount = Max(mistakesCount, out mistakeType);

        if (maxMistakeCount == -1)
        {
            currentEnding = Endings.Neutral;
        }
        else
        {
            switch (mistakeType)
            {
                case MistakeType.Sugar:
                    currentEnding = Endings.Sugar;
                    break;
                case MistakeType.Salt:
                    currentEnding = Endings.Salt;
                    break;
                case MistakeType.Fat:
                    currentEnding = Endings.Fat;
                    break;
                case MistakeType.Expired:
                    currentEnding = Endings.ExpiredOrDefect;
                    break;
                case MistakeType.Defect:
                    currentEnding = Endings.ExpiredOrDefect;
                    break;
                case MistakeType.WrongNutritionClaim:
                    currentEnding = Endings.WrongNutritionClaim;
                    break;
                case MistakeType.WrongCompositionClaim:
                    currentEnding = Endings.WrongCompositionClaim;
                    break;
                default:
                    currentEnding = Endings.Neutral; // Fallback for unexpected cases
                    break;
            }
        }

    }

    public void AddCorrect()
    {
        corrects++;
    }

    public void AddMistake()
    {
        mistakes++;
    }

    public void AddMistakeType(MistakeType type)
    {
        mistakeTypes.Add(type);
    }

    public void CheckProgress()
    {
        int progress = GameDataManager.gameData.levelProgress;
        int currentLevel = LevelManager.Instance.Level;
        int foodCount = LevelManager.Instance.ClientDataList.Count;

        if (currentLevel > progress && corrects == foodCount)
        {
            GameDataManager.gameData.IncreaseLevelProgress();
        }
    }

    private int Max(List<int> list, out MistakeType maxType)
    {
        int maxValue = list[0];
        maxType = MistakeType.None; // Initialize with a default value

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] ==  maxValue)
            {
                maxType = MistakeType.None; // Reset to default if there's a tie
                return -1;
            }
            if (list[i] > maxValue)
            {
                maxValue = list[i];
                maxType = (MistakeType)i; // Cast index to MistakeType
            }
        }
        return maxValue;
    }
}
