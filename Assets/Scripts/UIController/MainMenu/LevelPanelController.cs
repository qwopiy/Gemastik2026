using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanelController : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_3 = new(0.3f);
    private static readonly int IsOpenHash = Animator.StringToHash("isOpen");
    public List<Animator> panelAnimators;

    public void ShowLevelPanel(int levelIndex)
    {
        StartCoroutine(ShowLevelPanelCoroutine(levelIndex));
    }

    private IEnumerator ShowLevelPanelCoroutine(int levelIndex)
    {
        HideAllPanels();
        yield return _waitForSeconds0_3;
        for (int i = 0; i < panelAnimators.Count; i++)
        {
            panelAnimators[i].SetBool(IsOpenHash, i == levelIndex);
        }
    }

    public void HideAllPanels()
    {
        foreach (var animator in panelAnimators)
        {
            animator.SetBool(IsOpenHash, false);
        }
    }

    public void GoToLevel(int levelIndex)
    {
        string sceneName = $"Level{levelIndex + 1}";
        TransitionManager.Instance.GoToScene(sceneName);
    }
}
