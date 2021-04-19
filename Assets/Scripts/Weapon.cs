using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [Tooltip("The projectile to spawn.")]
    public Projectile Projectile;

    [Tooltip("The player-relative position to spawn a fired projectile at.")]
    public Vector3 FireOffset = new Vector3(0, 1, 1);

    [Tooltip("The velocity at which projectiles are fired.")]
    public Vector3 FireVelocity = new Vector3(0, 1, 5);

    /// <summary>
    /// Fires a projectile.
    /// </summary>
    /// <param name="pc">the player character</param>
    public void Fire(PlayerCharacter pc)
    {
        var direction = pc.transform.rotation;
        var projectile = Instantiate(Projectile, pc.transform.position + direction * FireOffset, direction);
        if (projectile.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.velocity = direction * FireVelocity;
        }
    }
}
