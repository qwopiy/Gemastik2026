using UnityEngine;
public class MainMenu : MonoBehaviour
{
    private GameObject currentPanel;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetCurrentPanel(GameObject panel)
    {
        currentPanel = panel;
    }

    public void GoToObject(GameObject obj)
    {
        TransitionManager.Instance.TransitionToGameObject(currentPanel, obj);
    }
}
