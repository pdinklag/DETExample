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

    [Header("UI")]
    [Tooltip("The pause menu prefab.")]
    public GameObject PauseMenuPrefab;

    [Tooltip("The text to be displayed when an item is picked up.")]
    public string PickupText = "Picked up {0}.";
}
