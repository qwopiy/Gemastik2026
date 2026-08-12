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
        isLevel4 = LevelManager.Instance.Level == 4;
    }

    public void SetStats()
    {
        int corrects = EndingManager.Instance.corrects;
        int foodAmount = LevelManager.Instance.FoodDataList.Count;

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
            //TransitionManager.Instance.GoToScene("EndingScene"); // TODO: Change to specific ending scenes
            return;
        }

        TransitionManager.Instance.GoToScene("LevelSelect");
    }
}
