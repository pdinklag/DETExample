using System.Collections;
using UnityEngine;

/// <summary>
/// A weapon that fires an instantly hitting ray.
/// </summary>
[CreateAssetMenu(fileName = "RayWeapon", menuName = "Weapon/Ray")]
public class RayWeapon : Weapon
{
    /// <summary>
    /// Interface for raycast hit listeners.
    /// </summary>
    public interface HitListener
    {
        /// <summary>
        /// Called when hit by ray weapon fire.
        /// </summary>
        /// <param name="weapon">the weapon</param>
        /// <param name="instigator">the player who fired</param>
        /// <param name="hit">information on the raycast hit</param>
        void OnHitByRayWeapon(RayWeapon weapon, PlayerCharacter instigator, RaycastHit hit);
    }

    [Header("Ray Weapon")]
    [Tooltip("The ray prefab to spawn.")]
    public LineRenderer RayEffectPrefab;

    [Tooltip("The maximum distance for the raycast.")]
    public float MaxRayDistance = 25.0f;

    public override IEnumerator CoFire(PlayerCharacter pc)
    {
        // get ray properties
        var rayOrigin = GetFirePosition(pc);
        var rayRotation = GetFireRotation(pc);

        // do a raycast to find where the line should end
        var rayDirection = rayRotation * Vector3.forward;
        var ray = new Ray(rayOrigin, rayDirection);
        var hits = Physics.RaycastAll(ray, MaxRayDistance);

        // spawn the ray effect
        var lineRenderer = RayEffectPrefab ? Instantiate(RayEffectPrefab) : null;

        // test if anything was hit at all
        if (hits.Length > 0)
        {
            // find closest hit
            RaycastHit closest = default; // nb: structs cannot be "null"
            var closestDistance = float.PositiveInfinity;
            foreach (var hit in hits)
            {
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closest = hit;
                }
            }

            // test and retrieve the hit collider for a listener component
            if (closest.collider.TryGetComponent<HitListener>(out var listener))
            {
                // notify listener
                listener.OnHitByRayWeapon(this, pc, closest);
            }

            // set line renderer endpoints
            if (lineRenderer)
            {
                lineRenderer.SetWorldSpacePoints(rayOrigin, closest.point);
            }
        }
        else
        {
            // nothing was hit

            // set line renderer endpoints using max distance
            if (lineRenderer)
            {
                lineRenderer.SetWorldSpacePoints(rayOrigin, rayOrigin + MaxRayDistance * rayDirection);
            }
        }

        // this coroutine finishes immediately
        yield break;
    }
}
