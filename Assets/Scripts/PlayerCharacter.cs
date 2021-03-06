using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A very simple player character.
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("The character's movement speed.")]
    public float Speed = 3.0f;

    [Tooltip("The character's rotation speed in degrees per second.")]
    public float RotSpeed = 270.0f;

    private Animator _anim;
    private CharacterController _controller;
    private Weapon _weapon;
    private bool _isFiring;
    private Vector2 _inputDirection;

    private float _currentSpeed = 0;
    private float _currentAcceleration = 0;

    private void Start()
    {
        // try to retrieve the controller
        if (!TryGetComponent(out _controller))
        {
            Debug.LogError("failed to find character controller", this);
            enabled = false; // disables Update
            return;
        }

        // try to retrieve the animator
        TryGetComponent(out _anim);
    }

    /// <summary>
    /// Gives the player a weapon.
    /// </summary>
    /// <param name="weapon">the weapon</param>
    public void GiveWeapon(Weapon weapon)
    {
        _weapon = weapon;

        // activate weapon layer in Animator
        if (_anim)
        {
            _anim.SetLayerWeight(1, weapon ? 1.0f : 0.0f);
        }
    }

    private void HandleMovement()
    {
        // get direction
        var direction = new Vector3(_inputDirection.x, 0, _inputDirection.y);

        // test if walking
        var walking = direction.magnitude > 0;

        // set flag in animator
        if (_anim)
        {
            _anim.SetBool("Walking", walking);
        }

        // update speed and acceleration
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, walking ? Speed : 0, ref _currentAcceleration, 0.125f);

        // move the character
        if (_currentSpeed > 0)
        {
            _controller.SimpleMove(_currentSpeed * direction);
        }

        // rotate the character
        if (walking)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), RotSpeed * Time.deltaTime);
        }
    }

    public void OnInputMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.performed ? context.ReadValue<Vector2>() : Vector2.zero;
    }

    public void OnInputFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_weapon && !_isFiring)
            {
                StartCoroutine(CoFireWeapon());
            }
        }
    }

    private IEnumerator CoFireWeapon()
    {
        // lock fire
        _isFiring = true;

        // set trigger in animator
        // reset trigger in animator
        if (_anim)
        {
            _anim.SetTrigger("Fire");
        }

        // start weapon's fire coroutine and wait for it
        yield return _weapon.CoFire(this);

        // wait for weapon's cooldown to expire
        yield return new WaitForSeconds(_weapon.FireCooldown);

        // reset trigger in animator
        if (_anim)
        {
            _anim.ResetTrigger("Fire");
        }

        // we are no longer firing
        _isFiring = false;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // test if we hit a wall trigger platform
        if (hit.collider.TryGetComponent<WallTriggerPlatform>(out var platform))
        {
            platform.Wall.Trigger();
        }
    }
}
