using System.Collections.Generic;
using UnityEngine;

public class LevelCalendarController : MonoBehaviour
{
    public List<GameObject> levelButtons; // Array of level button GameObjects
    private int currentLevelProgress = 0;
    private void Start()
    {
        GameDataManager.ReadData();

        currentLevelProgress = GameDataManager.gameData.levelProgress;
        UpdateLevelButtons();
    }

    public void UpdateLevelButtons()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].SetActive(i < currentLevelProgress);
        }
    }
}
