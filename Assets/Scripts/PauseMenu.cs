using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Implements pause menu logic.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Tooltip("The scene to load when exiting to the main menu.")]
    public string MainMenuSceneName;

    public void OnBackToGameClicked()
    {
        Level.Instance.PauseGame(false);
    }

    public void OnExitToMainMenuClicked()
    {
        Time.timeScale = 1.0f; // un-pause!
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
