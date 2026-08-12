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
        TransitionManager.Instance.TransitionToGameObject(levelCompletePanel);
        levelCompletePanel.GetComponent<LevelCompleteController>().SetStats();
    }
}