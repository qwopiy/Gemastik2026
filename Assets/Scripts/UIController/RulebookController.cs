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
        public List<GameObject> PageObjects;
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
        currentPageIndex = 0;
        DisplayPage(currentPageIndex);
    }

    public void NextPage()
    {
        if (currentPageIndex < rules.Count - 1)
        {
            currentPageIndex++;
            DisplayPage(currentPageIndex);
            AudioManager.Instance.TriggerPageFlip();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            DisplayPage(currentPageIndex);
            AudioManager.Instance.TriggerPageFlip();
        }
    }

    private void DisplayPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < rules.Count)
        {
            if (rules[pageIndex].LeftPage != null)
            {
                leftPage.gameObject.SetActive(true);
                leftPage.sprite = rules[pageIndex].LeftPage;
            }
            else
                leftPage.gameObject.SetActive(false);

            if (rules[pageIndex].RightPage != null)
            {
                rightPage.gameObject.SetActive(true);
                rightPage.sprite = rules[pageIndex].RightPage;
            }
            else
                rightPage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Invalid page index: " + pageIndex);
        }

        DeactivateAllPageObjects();
        ActivatePageObjects(pageIndex);

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

    private void DeactivateAllPageObjects()
    {
        foreach (var rule in rules)
        {
            foreach (var obj in rule.PageObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
    private void ActivatePageObjects(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < rules.Count)
        {
            foreach (var obj in rules[pageIndex].PageObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }

    public void Goto(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < rules.Count)
        {
            currentPageIndex = pageIndex;
            DisplayPage(currentPageIndex);
        }
        else
        {
            Debug.LogWarning("Invalid page index: " + pageIndex);
        }
    }
}
