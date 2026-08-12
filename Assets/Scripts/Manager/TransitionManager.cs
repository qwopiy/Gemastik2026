using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static readonly int StartHash = Animator.StringToHash("Start");
    private static readonly int EndHash = Animator.StringToHash("End");
    public Animator transitionAnimator;
    public float transitionDuration = 1f;
    public static TransitionManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(TransitionCoroutineToScene(sceneName));
    }

    private IEnumerator TransitionCoroutineToScene(string sceneName)
    {
        transitionAnimator.SetTrigger(EndHash);
        yield return new WaitForSeconds(transitionDuration);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        transitionAnimator.SetTrigger(StartHash);
    }
}
