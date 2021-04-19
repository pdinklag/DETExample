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

    [Tooltip("The player's movement speed.")]
    public float PlayerSpeed = 3.0f;

    [Header("Weapons")]
    [Tooltip("The available weapons.")]
    public Weapon[] Weapons;

    /// <summary>
    /// Gets the i-th weapon.
    /// </summary>
    /// <param name="i">the weapon number</param>
    /// <returns>the i-th weapon, or null if it does not exist</returns>
    public Weapon GetWeapon(int i) => (i >= 0 && i < Weapons.Length) ? Weapons[i] : null;
}
