using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteController : MonoBehaviour
{

    public TextMeshProUGUI statText;
    public TextMeshProUGUI timeText;
    public Image logoToChange;
    public Sprite correctAsset;
    public Sprite wrongAsset;
    private bool isLevel4;
    private void Start()
    {
        isLevel4 = LevelManager.Instance.Level >= 3;
    }

    public void SetStats()
    {
        int corrects = EndingManager.Instance.corrects;
        int foodAmount = 0;

        // Remove the last served client index from the list to avoid double counting
        LevelManager.Instance.ClientServedIndex.RemoveAt(LevelManager.Instance.ClientServedIndex.Count - 1);

        foreach (var foodList in LevelManager.Instance.ClientServedIndex)
        {
            foodAmount += LevelManager.Instance.ClientDataList[foodList].AmountToSpawn;
        }

        statText.text = $"Skor: {corrects}/{foodAmount}";

        if (corrects == foodAmount)
        {
            logoToChange.sprite = correctAsset;
            GameDataManager.gameData.levelProgress = Mathf.Max(GameDataManager.gameData.levelProgress, LevelManager.Instance.Level + 1);
            GameDataManager.SaveData();
        }
        else
        {
            logoToChange.sprite = wrongAsset;
        }

        SetTime();
    }
    public void SetTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(EndingManager.Instance.elapsedTime);
        timeText.text = $"Waktu: {time.Minutes}:{time.Seconds:D2}";
    }
    public void Continue()
    {
        GlobalManager.Instance.CurrentEnding = EndingManager.Instance.currentEnding;
        if (isLevel4) 
        {
            TransitionManager.Instance.GoToScene("Ending");
            return;
        }

        TransitionManager.Instance.GoToScene("MainMenu");
    }
}
