using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulebookController : MonoBehaviour
{
    [System.Serializable]
    public struct RulebookPages
    {
        public string PageTitle;
        public Sprite LeftPage;
        public Sprite RightPage;

        public RulebookPages(string title, Sprite left, Sprite right)
        {
            PageTitle = title;
            LeftPage = left;
            RightPage = right;
        }
    }

    [Header("Rulebook Pages")]
    public List<RulebookPages> rules;
    public Image leftPage;
    public Image rightPage;

    [Header("Navigation Buttons")]
    public Button rightButton;
    public Button leftButton;
    public int currentPageIndex = 0;

    private void Start()
    {
        leftButton.onClick.AddListener(PreviousPage);
        rightButton.onClick.AddListener(NextPage);

        currentPageIndex = 0;
        DisplayPage(currentPageIndex);
    }

    public void NextPage()
    {
        if (currentPageIndex < rules.Count - 1)
        {
            currentPageIndex++;
            DisplayPage(currentPageIndex);
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            DisplayPage(currentPageIndex);
        }
    }

    private void DisplayPage(int pageIndex)
    {
        if (pageIndex == 0)
        {
            leftPage.gameObject.SetActive(false);
            rightPage.sprite = rules[pageIndex].RightPage;
        }

        if (pageIndex >= 1 && pageIndex < rules.Count)
        {
            leftPage.gameObject.SetActive(true);
            rightPage.gameObject.SetActive(true);

            leftPage.sprite = rules[pageIndex].LeftPage;
            rightPage.sprite = rules[pageIndex].RightPage;
        }
        else
        {
            Debug.LogWarning("Invalid page index: " + pageIndex);
        }

        leftButton.interactable = HasPreviousPage();
        rightButton.interactable = HasNextPage();
    }

    private bool HasNextPage()
    {
        return currentPageIndex < rules.Count - 1;
    }
    
    private bool HasPreviousPage()
    {
        return currentPageIndex > 0;
    }
}
