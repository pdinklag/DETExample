using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract base for weapons.
/// </summary>
public abstract class Weapon : ScriptableObject
{
    [Header("Weapon")]
    [Tooltip("The display name of the weapon.")]
    public string DisplayName;

    [Tooltip("The color to use when displaying the weapon name.")]
    public Color DisplayTextColor;
    
    [Tooltip("The player-relative position to spawn a fired projectile or ray at.")]
    public Vector3 FireOffset = new Vector3(0, 1, 1);

    [Tooltip("After the weapon has fired, this many seconds need to pass before the player can fire again.")]
    public float FireCooldown = 0.5f;

    /// <summary>
    /// Gets the initial rotation for spawned projectiles or other effects.
    /// </summary>
    /// <param name="pc">the firing player</param>
    /// <returns>the fire rotation</returns>
    protected Quaternion GetFireRotation(PlayerCharacter pc) => pc.transform.rotation;

    /// <summary>
    /// Gets the initial position for spawned projectiles or other effects.
    /// </summary>
    /// <param name="pc">the firing player</param>
    /// <returns>the fire position</returns>
    protected Vector3 GetFirePosition(PlayerCharacter pc) => pc.transform.position + GetFireRotation(pc) * FireOffset;

    /// <summary>
    /// Fires the weapon.
    /// </summary>
    /// <param name="pc">the player character firing the weapon</param>
    /// <returns>the coroutine enumerator</returns>
    public abstract IEnumerator CoFire(PlayerCharacter pc);

    /// <summary>
    /// Gets the display name with color tag for display.
    /// </summary>
    /// <returns>the colored display name</returns>
    public string GetColoredDisplayName() => string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(DisplayTextColor), DisplayName);
}
