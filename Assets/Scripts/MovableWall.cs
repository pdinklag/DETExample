using System.Collections;
using UnityEngine;

/// <summary>
/// A wall that moves down when shot.
/// </summary>
[RequireComponent(typeof(Animator))]
public class MovableWall : MonoBehaviour
{
    private static readonly string TRIGGER_MOVE_DOWN = "MoveDown";
    private static readonly string TRIGGER_MOVE_UP = "MoveUp";

    [Tooltip("The weapon that must be used to trigger the wall.")]
    public string TriggerProjectileID = "RedMarble";

    [Tooltip("The number of seconds that the wall stays down.")]
    public float DownDuration = 3.0f;

    private Animator _animator;
    private bool _moving;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // make sure we are not already moving
        if (!_moving)
        {
            // test if the hitting collider is a projectile
            if (collision.collider.TryGetComponent<Projectile>(out var proj))
            {
                // if so, make sure it is the correct type of projectile
                if (proj.ID == TriggerProjectileID)
                {
                    // start the trigger coroutine
                    StartCoroutine(CoTrigger());
                }
            }
        }
    }

    private IEnumerator CoTrigger()
    {
        // flag the wall as moving
        _moving = true;

        // fire the "MoveDown" trigger in the Animator state machine
        _animator.SetTrigger(TRIGGER_MOVE_DOWN);

        // wait before moving back up
        yield return new WaitForSeconds(DownDuration);

        // fire the "MoveUp" trigger
        _animator.SetTrigger(TRIGGER_MOVE_UP);

        // wait until the transition is completed
        do
        {
            yield return null;
        } while (_animator.IsInTransition(0));
        // EXERCISE: what happens if we turn the loop above into a head-controlled loop rather than a tail-controlled loop?

        // reset triggers and moving flag
        _animator.ResetTrigger(TRIGGER_MOVE_UP);
        _animator.ResetTrigger(TRIGGER_MOVE_DOWN);
        _moving = false;
    }
}
