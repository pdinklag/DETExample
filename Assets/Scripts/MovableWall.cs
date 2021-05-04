using System.Collections;
using UnityEngine;

/// <summary>
/// A wall that moves down when shot.
/// </summary>
public class MovableWall : MonoBehaviour, RayWeapon.HitListener
{
    private static readonly string TRIGGER_MOVE_DOWN = "MoveDown";
    private static readonly string TRIGGER_MOVE_UP = "MoveUp";

    [Tooltip("The animator to use for animation.")]
    public Animator Animator;

    [Tooltip("The weapon that must be used to trigger the wall.")]
    public string TriggerProjectileID = "RedMarble";

    [Tooltip("The ray weapon that must be used to trigger the wall.")]
    public RayWeapon TriggerRayWeapon;

    [Tooltip("The number of seconds that the wall stays down.")]
    public float DownDuration = 3.0f;

    private bool _moving;

    private void Trigger()
    {
        // make sure we are not already moving
        if (!_moving)
        {
            // start the trigger coroutine
            StartCoroutine(CoTrigger());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // test if the hitting collider is a projectile
        if (collision.collider.TryGetComponent<Projectile>(out var proj))
        {
            // if so, make sure it is the correct type of projectile
            if (proj.ID == TriggerProjectileID)
            {
                Trigger();
            }
        }
    }

    private IEnumerator CoTrigger()
    {
        // flag the wall as moving
        _moving = true;

        // fire the "MoveDown" trigger in the Animator state machine
        Animator.SetTrigger(TRIGGER_MOVE_DOWN);

        // wait before moving back up
        yield return new WaitForSeconds(DownDuration);

        // fire the "MoveUp" trigger
        Animator.SetTrigger(TRIGGER_MOVE_UP);

        // wait until the transition is completed
        do
        {
            yield return null;
        } while (Animator.IsInTransition(0));
        // EXERCISE: what happens if we turn the loop above into a head-controlled loop rather than a tail-controlled loop?

        // reset triggers and moving flag
        Animator.ResetTrigger(TRIGGER_MOVE_UP);
        Animator.ResetTrigger(TRIGGER_MOVE_DOWN);
        _moving = false;
    }

    public void OnHitByRayWeapon(RayWeapon weapon, PlayerCharacter instigator, RaycastHit hit)
    {
        // test if hit by the correct ray weapon
        if (weapon == TriggerRayWeapon)
        {
            Trigger();
        }
    }
}
