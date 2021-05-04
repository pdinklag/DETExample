using UnityEngine;

/// <summary>
/// The game's settings.
/// </summary>
[CreateAssetMenu(fileName = "Settings", menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    /// <summary>
    /// Loads the game settings from the Resources directory.
    /// </summary>
    /// <returns></returns>
    public static GameSettings Load() => Resources.Load<GameSettings>("Settings");

    [Header("Player")]
    [Tooltip("The player prefab.")]
    public PlayerCharacter PlayerPrefab;

    [Header("UI")]
    [Tooltip("The pause menu prefab.")]
    public GameObject PauseMenuPrefab;
}
