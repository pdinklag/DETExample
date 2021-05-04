using System.Collections;
using UnityEngine;

/// <summary>
/// A very simple player character.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("The character's movement speed.")]
    public float Speed = 3.0f;

    private CharacterController _controller;
    private Weapon _weapon;
    private bool _isFiring;

    private void Start()
    {
        // try to retrieve the controller
        if (!TryGetComponent(out _controller))
        {
            Debug.LogError("failed to find character controller", this);
            enabled = false; // disables Update
        }
    }

    /// <summary>
    /// Gives the player a weapon.
    /// </summary>
    /// <param name="weapon">the weapon</param>
    public void GiveWeapon(Weapon weapon)
    {
        _weapon = weapon;
    }

    private void HandleMovement()
    {
        // get input axes
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        // compute move direction
        var direction = new Vector3(x, 0, z).normalized;

        // move the character
        _controller.SimpleMove(Speed * direction);

        // rotate the character
        if (x != 0 || z != 0)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void HandleWeapon()
    {
        if (_weapon && !_isFiring && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(CoFireWeapon());
        }
    }

    private IEnumerator CoFireWeapon()
    {
        // lock fire
        _isFiring = true;

        // start weapon's fire coroutine and wait for it
        yield return _weapon.CoFire(this);

        // wait for weapon's cooldown to expire
        yield return new WaitForSeconds(_weapon.FireCooldown);

        // we are no longer firing
        _isFiring = false;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        HandleWeapon();
    }
}
