using UnityEngine;

public class PausePanelController : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the pause panel UI
    private void Start()
    {
        GlobalManager.Instance.EscapePressedEvent += TogglePause;

        pausePanel.SetActive(false); // Ensure the pause panel is hidden at the start
    }

    private void OnDisable()
    {
        GlobalManager.Instance.EscapePressedEvent -= TogglePause;
    }

    private void TogglePause()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        pausePanel.SetActive(true); // Show the pause panel
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        pausePanel.SetActive(false); // Hide the pause panel
    }

    public void ExitLevel()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when exiting
        EndingManager.Instance.StopTimer();
        EndingManager.Instance.ResetEndingData();

        TransitionManager.Instance.GoToScene("MainMenu"); // Transition to the main menu
    }
}
