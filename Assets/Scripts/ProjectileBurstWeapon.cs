using System.Collections;
using UnityEngine;

/// <summary>
/// A weapon that fires projectiles in a burst.
/// </summary>
public class ProjectileBurstWeapon : ProjectileWeapon
{
    [Header("Projectile Burst")]
    [Tooltip("The number of projectiles to fire in a burst.")]
    public int BurstCount = 3;

    [Tooltip("The number of seconds between two burst shots.")]
    public float BurstInterval = 0.5f;

    public override IEnumerator CoFire(PlayerCharacter pc)
    {
        for(var shot = 0; shot < BurstCount; shot++)
        {
            if(shot > 0)
            {
                // unless this is the first shot,
                // let the burst interval expire before firing the next shot
                yield return new WaitForSeconds(BurstInterval);
            }

            // fire next shot using base class
            yield return base.CoFire(pc);
        }
    }
}
