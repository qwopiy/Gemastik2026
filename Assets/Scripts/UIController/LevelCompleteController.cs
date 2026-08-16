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
            TransitionManager.Instance.GoToScene("Ending");
            return;
        }

        TransitionManager.Instance.GoToScene("MainMenu");
    }
}
