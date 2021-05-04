using UnityEngine;

/// <summary>
/// Implements pause menu logic.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public void OnBackToGameClicked()
    {
        Level.Instance.PauseGame(false);
    }

    public void OnExitToMainMenuClicked()
    {
        // TODO
    }
}
