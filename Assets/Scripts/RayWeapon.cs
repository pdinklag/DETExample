using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RayWeapon", menuName = "Weapon/Ray")]
public class RayWeapon : Weapon
{
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
