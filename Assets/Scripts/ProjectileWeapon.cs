using System.Collections;
using UnityEngine;

/// <summary>
/// A weapon that fires projectiles.
/// </summary>
[CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "Weapon/Projectile")]
public class ProjectileWeapon : Weapon
{
    [Header("Projectile Weapon")]
    [Tooltip("The projectile to spawn.")]
    public Projectile Projectile;

    [Tooltip("The velocity at which projectiles are fired.")]
    public Vector3 FireVelocity = new Vector3(0, 1, 5);

    public override IEnumerator CoFire(PlayerCharacter pc)
    {
        var direction = pc.transform.rotation;
        var projectile = Instantiate(Projectile, GetFirePosition(pc), GetFireRotation(pc));
        if (projectile.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.velocity = direction * FireVelocity;
        }

        // this coroutine finishes immediately
        yield break;
    }
}
