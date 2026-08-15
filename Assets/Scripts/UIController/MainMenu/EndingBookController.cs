using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingBookController : MonoBehaviour
{
    public List<EndingPageData> endingPages;


    public void Start()
    {
        GameDataManager.ReadData();

        UpdateEndings();
    }

    private void UpdateEndings()
    {
        UpdateEndingUnlocked(Endings.Neutral, GameDataManager.gameData.neutralEndingUnlocked);
        UpdateEndingUnlocked(Endings.AllMistake, GameDataManager.gameData.allMistakeEndingUnlocked);
        UpdateEndingUnlocked(Endings.AllCorrect, GameDataManager.gameData.allCorrectEndingUnlocked);
        UpdateEndingUnlocked(Endings.Sugar, GameDataManager.gameData.sugarEndingUnlocked);
        UpdateEndingUnlocked(Endings.Salt, GameDataManager.gameData.saltEndingUnlocked);
        UpdateEndingUnlocked(Endings.Fat, GameDataManager.gameData.fatEndingUnlocked);
        UpdateEndingUnlocked(Endings.ExpiredOrDefect, GameDataManager.gameData.defectOrExpiredEndingUnlocked);
        UpdateEndingUnlocked(Endings.WrongNutritionClaim, GameDataManager.gameData.wrongNutritionClaimEndingUnlocked);
        UpdateEndingUnlocked(Endings.WrongCompositionClaim, GameDataManager.gameData.wrongCompositionClaimEndingUnlocked);
        UpdateEndingUnlocked(Endings.PerfectSpeedrunner, GameDataManager.gameData.perfectSpeedrunnerEndingUnlocked);
    }
    private void UpdateEndingUnlocked(Endings ending, bool isUnlocked)
    {
        foreach (var page in endingPages)
        {
            if (page.type == ending)
            {
                if (isUnlocked)
                {
                    page.UnlockPage();
                }
                else
                {
                    page.LockPage();
                }
                break;
            }
        }
    }
}

[Serializable]
public class EndingPageData
{
    public Endings type;
    
    [Header("Page Title")]
    public TextMeshProUGUI pageTitleTMP;
    public string pageTitleUnlocked;
    public string pageTitleLocked;

    [Header("Page Content")]
    public TextMeshProUGUI pageContentTMP;
    public string pageContentUnlocked;
    public string pageContentLocked;

    [Header("Page Settings")]

    public Sprite endingSprite;
    public Sprite endingSpriteLocked;
    public Image endingImage;
    public bool isUnlocked;

    public void UnlockPage()
    {
        isUnlocked = true;
        endingImage.sprite = endingSprite;
        pageTitleTMP.text = pageTitleUnlocked;
        pageContentTMP.text = pageContentUnlocked;
    }

    public void LockPage()
    {
        isUnlocked = false;
        endingImage.sprite = endingSpriteLocked;
        pageTitleTMP.text = pageTitleLocked;
        pageContentTMP.text = pageContentLocked;
    }
}