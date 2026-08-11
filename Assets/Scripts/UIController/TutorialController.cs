using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialController : MonoBehaviour, IPointerClickHandler
{
    public GameObject tutorialPanel;
    public List<GameObject> tutorialObjects;
    public int index;

    private void Start()
    {
        for (int i = 0; i < tutorialObjects.Count; i++)
        {
            tutorialObjects[i].SetActive(false);
        }

        if (tutorialObjects.Count > 0)
        {
            tutorialObjects[0].SetActive(true);
            index = 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ContinueTutorial();
    }

    private void ContinueTutorial()
    {
        if (index < tutorialObjects.Count - 1)
        {
            tutorialObjects[index].SetActive(false);
            index++;
            tutorialObjects[index].SetActive(true);
        }
        else
        {
            tutorialObjects[index].SetActive(false);
            tutorialPanel.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
