using System.Collections;
using UnityEngine; 
public class GameStarter : MonoBehaviour
{
    private static readonly int HideBoxHash = Animator.StringToHash("HideBox");
    private static readonly int ShowBoxHash = Animator.StringToHash("ShowBox");
    public Animator animator;
    public GameObject levelCompletePanel;

    private void Start()
    {
        LevelManager.Instance.LevelCompletedEvent += ShowBox;
    }
    private void OnDisable()
    {
        LevelManager.Instance.LevelCompletedEvent -= ShowBox;
    }
    public void ShowBox()
    {
        animator.SetTrigger(ShowBoxHash);
    }
    public void HideBox()
    {
        animator.SetTrigger(HideBoxHash);
    }

    public void StartDay()
    {
        EndingManager.Instance.ResetEndingData();

        LevelManager.Instance.TriggerLevelStartedEvent();
        DialogueEventManager.Instance.TriggerStartEvent();
    }

    public void EndDay()
    {
        TransitionManager.Instance.TransitionToGameObject(gameObject, levelCompletePanel);
        levelCompletePanel.GetComponent<LevelCompleteController>().SetStats();
    }

    public void DisableButton(GameObject button)
    {
        StartCoroutine(DisableButtonCoroutine(button));
    }
    private IEnumerator DisableButtonCoroutine(GameObject button)
    {
        yield return new WaitForSeconds(1f);
        button.SetActive(false);
    }
}