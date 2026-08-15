using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteController : MonoBehaviour
{

    public TextMeshProUGUI statText;
    public Image logoToChange;
    public Sprite correctAsset;
    public Sprite wrongAsset;
    private bool isLevel4;
    private void Start()
    {
        isLevel4 = LevelManager.Instance.Level == 3;
    }

    public void SetStats()
    {
        int corrects = EndingManager.Instance.corrects;
        int foodAmount = 0;

        foreach (var foodList in LevelManager.Instance.ClientDataList)
        {
            foodAmount += foodList.AmountToSpawn;
        }

        statText.text = $"Jumlah makanan yang benar: {corrects}/{foodAmount}";

        if (corrects == foodAmount)
        {
            logoToChange.sprite = correctAsset;
        }
        else
        {
            logoToChange.sprite = wrongAsset;
        }
    }
    public void Continue()
    {
        if (isLevel4) 
        {
            string endingSceneName = GetEndingName();
            TransitionManager.Instance.GoToScene(endingSceneName);
            return;
        }

        TransitionManager.Instance.GoToScene("MainMenu");
    }

    private string GetEndingName() // TODO: Change to specific ending scenes
    {
        Endings ending = EndingManager.Instance.currentEnding;

        switch (ending)
        {
            case Endings.AllCorrect:
                return "EndingAllCorrect";
            case Endings.AllMistake:
                return "EndingAllMistake";
            case Endings.Sugar:
                return "EndingSugar";
            case Endings.Salt:
                return "EndingSalt";
            case Endings.Fat:
                return "EndingFat";
            case Endings.ExpiredOrDefect:
                return "EndingExpiredOrDefect";
            case Endings.WrongNutritionClaim:
                return "EndingWrongNutritionClaim";
            case Endings.WrongCompositionClaim:
                return "EndingWrongCompositionClaim";
            case Endings.PerfectSpeedrunner:
                return "EndingPerfectSpeedrunner";
            default:
                Debug.LogError("Unknown ending: " + ending);
                return "LevelSelect"; // Fallback to LevelSelect if unknown
        }
    }
}
