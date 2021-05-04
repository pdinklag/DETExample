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

    [Header("Weapons")]
    [Tooltip("The character's LMB weapon.")]
    public Weapon PrimaryWeapon;

    [Tooltip("The character's RMB weapon.")]
    public Weapon SecondaryWeapon;

    private CharacterController _controller;
    private Weapon _isFiring;

    private void Start()
    {
        // try to retrieve the controller
        if (!TryGetComponent(out _controller))
        {
            Debug.LogError("failed to find character controller", this);
            enabled = false; // disables Update
        }
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
        if (PrimaryWeapon && Input.GetButtonDown("Fire1"))
        {
            // fire first weapon
            FireWeapon(PrimaryWeapon);
        }
        else if (SecondaryWeapon && Input.GetButtonDown("Fire2"))
        {
            // fire second weapon
            FireWeapon(SecondaryWeapon);
        }
    }

    private void FireWeapon(Weapon w)
    {
        if (!_isFiring)
        {
            // start fire in a coroutine, unless already firing with another weapon
            StartCoroutine(CoFireWeapon(w));
        }
    }

    private IEnumerator CoFireWeapon(Weapon w)
    {
        // lock fire
        _isFiring = w;

        // start weapon's fire coroutine and wait for it
        yield return w.CoFire(this);

        // wait for weapon's cooldown to expire
        yield return new WaitForSeconds(w.FireCooldown);

        // we are no longer firing
        _isFiring = null;
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
