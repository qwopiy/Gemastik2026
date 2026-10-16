using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCalendarController : MonoBehaviour
{
    public List<Button> levelButtons; // Array of level button GameObjects
    public List<GameObject> redCircles;
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
            levelButtons[i].interactable = i <= currentLevelProgress;
            redCircles[i].SetActive(i <= currentLevelProgress);
        }
    }
}
