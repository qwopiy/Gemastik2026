using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingPanelController : MonoBehaviour
{
    private static readonly int StartHash = Animator.StringToHash("Start");
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);
    private static readonly WaitForSeconds _waitForSeconds0_05 = new(0.05f);

    [Header("References")]
    [SerializeField] private Image endingImage;
    [SerializeField] private List<Sprite> endingSprites;
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private GameObject goodEndingPanel;
    [SerializeField] private GameObject badEndingPanel;
    [SerializeField] private TMPro.TextMeshProUGUI endingTextUI;
    [SerializeField] private GameObject continueButton;

    [SerializeField] private List<Animator> endingAnimators;

    private bool isGoodEnding = false;
    private string endingText = "Kamu Berhasil Mendapatkan \n";

    private void Start()
    {
        GetEnding();

        StartCoroutine(EndingSequenceCoroutine());
    }

    public void GoToMainMenu()
    {
        GameDataManager.gameData.UnlockEnding(GlobalManager.Instance.CurrentEnding);

        TransitionManager.Instance.GoToScene("MainMenu");
    }

    private void GetEnding()
    {
        isGoodEnding = GlobalManager.Instance.CurrentEnding switch
        {
            Endings.AllCorrect or Endings.PerfectSpeedrunner => true,
            _ => false,
        };

        endingText += GlobalManager.Instance.CurrentEnding switch
        {
            Endings.Neutral => "Ending 1: Depresei",
            Endings.AllMistake => "Ending 2: Perang Dunia",
            Endings.AllCorrect => "Ending 3: Perdamaian Dunia",
            Endings.Sugar => "Ending 4: Obesitas + Diabetes",
            Endings.Salt => "Ending 5: Kegaraman",
            Endings.Fat => "Ending 6: Struk",
            Endings.ExpiredOrDefect => "Ending 7: Muntah",
            Endings.WrongNutritionClaim => "Ending 8: Pembunuh tersembunyi",
            Endings.WrongCompositionClaim => "Ending 9: Pendek",
            Endings.PerfectSpeedrunner => "Ending 10: Speedrunner",
            _ => "Unknown ending."
        };

        endingImage.sprite = endingSprites[(int)GlobalManager.Instance.CurrentEnding];
    }

    private IEnumerator EndingSequenceCoroutine()
    {
        if (isGoodEnding)
        {
            goodEndingPanel.SetActive(true);
        }
        else
        {
            badEndingPanel.SetActive(true);
        }

        // Wait for transition
        yield return _waitForSeconds1;

        foreach (var animator in endingAnimators)
        {
            animator.SetTrigger(StartHash);
        }

        yield return TypeLinesCoroutine(endingText);

        continueButton.SetActive(true);
    }

    private IEnumerator TypeLinesCoroutine(string text)
    {
        foreach (char c in text.ToCharArray())
        {
            endingTextUI.text += c;
            yield return _waitForSeconds0_05;
        }
    }
}
