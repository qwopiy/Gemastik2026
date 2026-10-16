using System;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [Header("References")]
    public TMPro.TextMeshProUGUI dateText;
    public TMPro.TextMeshProUGUI clockText;

    private void Update()
    {
        dateText.text = GetCurrentLevel().ToString("D2") + "/12/2026";
        clockText.text = GetCurrentTime().ToString(@"hh\:mm\:ss");
    }
    private int GetCurrentLevel()
    {
        int currentLevel = LevelManager.Instance.Level switch
        {
            0 => 3,
            1 => 5,
            2 => 17,
            3 => 20,
            // Add more cases as needed
            _ => 1
        };
        return currentLevel;
    }

    private TimeSpan GetCurrentTime()
    {
        return TimeSpan.FromSeconds(EndingManager.Instance.elapsedTime);
    }
}
