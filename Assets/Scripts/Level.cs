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

    [Tooltip("Where to spawn the player.")]
    public Transform SpawnPoint;

    [Tooltip("The camera controller.")]
    public CameraController Camera;

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
    }

    private void Start()
    {
        // spawn the player
        var pc = Instantiate(Settings.PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation);
        Camera.Player = pc;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
