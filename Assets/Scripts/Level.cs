using UnityEngine;

/// <summary>
/// An object containing information about the current level.
/// </summary>
public class Level : MonoBehaviour
{
    /// <summary>
    /// The singleton instance.
    /// </summary>
    public static Level Instance { get; private set; }

    /// <summary>
    /// The game settings.
    /// </summary>
    public GameSettings Settings { get; private set; }

    /// <summary>
    /// Is the game currently paused?
    /// </summary>
    public bool Paused { get; private set; }

    [Tooltip("Where to spawn the player.")]
    public Transform SpawnPoint;

    [Tooltip("The camera controller.")]
    public CameraController Camera;

    private GameObject _pauseMenu;

    private void Awake()
    {
        // there can be only one...
        if (Instance)
        {
            Debug.LogError("only one Level instance allowed");
            Destroy(gameObject); // exercise: what would be different if we used Destroy(this) instead?
            return;
        }
        else
        {
            Instance = this;
        }

        // load settings
        Settings = GameSettings.Load();

        // instantiate the pause menu and deactivate it
        _pauseMenu = Instantiate(Settings.PauseMenuPrefab, transform);
        _pauseMenu.SetActive(false);
    }

    private void Start()
    {
        // spawn the player
        var pc = Instantiate(Settings.PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation);
        Camera.Player = pc;
    }

    public void PauseGame(bool pause)
    {
        // set paused flag
        Paused = pause;

        // activate or deactivate the pause menu
        _pauseMenu.SetActive(Paused);

        // adjust the time scale accordingly
        // CAUTION: even when the time scale is zero, Update will be called on objects (with Time.deltaTime == 0)
        Time.timeScale = Paused ? 0.0f : 1.0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(!Paused);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
