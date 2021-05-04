using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// The main menu.
/// </summary>
public class MainMenu : UIBehaviour, ICancelHandler
{
    [Tooltip("The name of the scene to load when Start is clicked.")]
    public string StartSceneName;

    public void OnStartClicked()
    {
        SceneManager.LoadScene(StartSceneName, LoadSceneMode.Single);
    }

    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Quit");
#endif

        Application.Quit(); // note: this does nothing in the Editor
    }

    public void OnCancel(BaseEventData eventData)
    {
        OnQuitClicked();
    }
}
