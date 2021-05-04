using System.Collections;
using UnityEngine;

/// <summary>
/// A weapon pickup that gives the player a weapon when entered.
/// </summary>
public class WeaponPickup : MonoBehaviour
{
    [Tooltip("The weapon to grant.")]
    public Weapon Weapon;

    [Tooltip("The number of seconds after which to respawn the pickup after being picked up.")]
    public float RespawnTime = 5.0f;

    [Tooltip("The visual item to be disabled when picked up.")]
    public GameObject Item;

    private bool _pickedUp;

    private IEnumerator CoPickup()
    {
        // disable
        _pickedUp = true;
        if (Item)
        {
            Item.SetActive(false);
        }

        // wait
        yield return new WaitForSeconds(RespawnTime);

        // re-enable
        if (Item)
        {
            Item.SetActive(true);
        }
        _pickedUp = false;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (!_pickedUp)
        {
            if (c.TryGetComponent<PlayerCharacter>(out var pc))
            {
                // give weapon to player
                pc.GiveWeapon(Weapon);

                // pick up and respawn
                StartCoroutine(CoPickup());
            }
        }
    }
}
