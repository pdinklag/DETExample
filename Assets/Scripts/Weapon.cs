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

    [Tooltip("After the weapon has fired, this many seconds need to pass before the player can fire again.")]
    public float FireCooldown = 0.5f;

    /// <summary>
    /// Fires the weapon.
    /// </summary>
    /// <param name="pc">the player character firing the weapon</param>
    /// <returns>the coroutine enumerator</returns>
    public abstract IEnumerator CoFire(PlayerCharacter pc);
}
