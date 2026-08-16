using UnityEngine;
public class MainMenu : MonoBehaviour
{
    private GameObject currentPanel;
    private void Start()
    {
        GameDataManager.ReadData();
    }
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

    public void StartBGMLevel(int index)
    {
        AudioClip clip = index switch
        {
            0 => AudioManager.Instance.CFD,
            1 => AudioManager.Instance.Kantin,
            2 => AudioManager.Instance.EventCosplay,
            3 => AudioManager.Instance.Kantor,
            _ => null
        };

        if (clip != null)
        {
            AudioManager.Instance.PlayMusic(clip);
        }
    }
}
