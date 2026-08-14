using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_2 = new WaitForSeconds(0.2f);
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

    public void TransitionToGameObject(GameObject targetObject)
    {
        StartCoroutine(TransitionCoroutineToGameObject(targetObject));
    }

    private IEnumerator TransitionCoroutineToScene(string sceneName)
    {
        transitionAnimator.SetTrigger(EndHash);
        yield return new WaitForSeconds(transitionDuration);
        yield return LoadSceneCoroutine(sceneName);
        transitionAnimator.SetTrigger(StartHash);
    }

    private IEnumerator TransitionCoroutineToGameObject(GameObject targetObject)
    {
        transitionAnimator.SetTrigger(EndHash);
        yield return new WaitForSeconds(transitionDuration);
        targetObject.SetActive(true);
        transitionAnimator.SetTrigger(StartHash);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Begin loading the scene asynchronously in the background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully finishes loading
        while (!asyncLoad.isDone)
        {
            // Optional: Get loading progress (0.0 to 0.9)
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log($"Loading progress: {progress * 100}%");

            yield return null; // Wait until the next frame
        }

        yield return _waitForSeconds0_2; // Optional: Small delay to ensure scene is fully initialized

        // Code executed here runs immediately AFTER the scene has fully loaded
        Debug.Log("Scene fully loaded!");
    }
}
