using System.Collections;
using UnityEngine;

/// <summary>
/// A very simple player character.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
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
        _controller.SimpleMove(Level.Instance.Settings.PlayerSpeed * direction);

        // rotate the character
        if (x != 0 || z != 0)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void HandleWeapon()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // fire first weapon
            FireWeapon(Level.Instance.Settings.GetWeapon(0));
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            // fire second weapon
            FireWeapon(Level.Instance.Settings.GetWeapon(1));
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
